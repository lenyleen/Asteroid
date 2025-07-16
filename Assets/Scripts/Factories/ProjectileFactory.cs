using System;
using System.Collections.Generic;
using System.Linq;
using DataObjects;
using Interfaces;
using Projectiles;
using UnityEngine;
using Zenject;

namespace Factories
{
    public class ProjectileFactory : IFactory<ProjectileType,float,FireData,IProjectile>
    {
        private readonly Dictionary<ProjectileType,ProjectileData> _projectileDatas;
        private readonly Projectile.Pool _pool;
        
        public ProjectileFactory(Projectile.Pool pool ,List<ProjectileData> projectileDatas)
        {
            _pool = pool;
            _projectileDatas = projectileDatas.ToDictionary(data => data.Type, data => data );
        }
        
        public IProjectile Create(ProjectileType type,float rotation, FireData fireData)
        {
            if (!_projectileDatas.TryGetValue(type, out var data))
                throw new Exception($"ProjectileData not found for type: {type}");

            var projectile = _pool.Spawn(data.Sprite,rotation, prj => _pool.Despawn(prj));
            projectile.ApplyBehaviour(fireData.Behaviour);
            return projectile;
        }
        
    }
}