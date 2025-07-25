﻿using System;
using System.Collections.Generic;
using System.IO;
using Zenject.ReflectionBaking.Mono.Cecil;

namespace Zenject.ReflectionBaking
{
    public class UnityAssemblyResolver : BaseAssemblyResolver
    {
        private readonly IDictionary<string, string> _appDomainAssemblyLocations;
        private readonly IDictionary<string, AssemblyDefinition> _cache;

        public UnityAssemblyResolver()
        {
            _appDomainAssemblyLocations = new Dictionary<string, string>();
            _cache = new Dictionary<string, AssemblyDefinition>();

            var domain = AppDomain.CurrentDomain;

            var assemblies = domain.GetAssemblies();

            for (var i = 0; i < assemblies.Length; i++)
            {
#if NET_4_6
                if (assemblies[i].IsDynamic) continue;
#endif

                _appDomainAssemblyLocations[assemblies[i].FullName] = assemblies[i].Location;

                AddSearchDirectory(Path.GetDirectoryName(assemblies[i].Location));
            }
        }

        public override AssemblyDefinition Resolve(AssemblyNameReference name)
        {
            var assemblyDef = FindAssemblyDefinition(name.FullName, null);

            if (assemblyDef == null)
            {
                assemblyDef = base.Resolve(name);
                _cache[name.FullName] = assemblyDef;
            }

            return assemblyDef;
        }

        public override AssemblyDefinition Resolve(AssemblyNameReference name, ReaderParameters parameters)
        {
            var assemblyDef = FindAssemblyDefinition(name.FullName, parameters);

            if (assemblyDef == null)
            {
                assemblyDef = base.Resolve(name, parameters);
                _cache[name.FullName] = assemblyDef;
            }

            return assemblyDef;
        }

        /// Searches for AssemblyDefinition in our cache, and failing that,
        /// looks for a known location.  Returns null if both attempts fail.
        private AssemblyDefinition FindAssemblyDefinition(string fullName, ReaderParameters parameters)
        {
            if (fullName == null) throw new ArgumentNullException("fullName");

            AssemblyDefinition assemblyDefinition;

            // Look in cache first
            if (_cache.TryGetValue(fullName, out assemblyDefinition)) return assemblyDefinition;

            // Try to use known location

            string location;

            if (_appDomainAssemblyLocations.TryGetValue(fullName, out location))
            {
                if (parameters != null)
                    assemblyDefinition = AssemblyDefinition.ReadAssembly(location, parameters);
                else
                    assemblyDefinition = AssemblyDefinition.ReadAssembly(location);

                _cache[fullName] = assemblyDefinition;

                return assemblyDefinition;
            }

            return null;
        }
    }
}