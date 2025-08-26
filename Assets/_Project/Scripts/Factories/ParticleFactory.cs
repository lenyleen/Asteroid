using System;
using System.Collections.Generic;
using _Project.Scripts.Configs;
using _Project.Scripts.Data;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Other;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Factories
{
    public class ParticleFactory : ISceneInitializable
    {
        private const string ConfigsListKey = "VfxConfigs";

        private readonly IScenesAssetProvider _assetProvider;
        private readonly IRemoteConfigService _remoteConfigService;
        private readonly IInstantiator _instantiator;

        private Dictionary<VfxType, string> _particlePrefabAddresses = new ();

        public ParticleFactory(IScenesAssetProvider assetProvider, IRemoteConfigService remoteConfigService,
            IInstantiator instantiator)
        {
            _assetProvider = assetProvider;
            _remoteConfigService = remoteConfigService;
            _instantiator = instantiator;
        }

        public UniTask InitializeAsync()
        {
            var availableConfigs = _remoteConfigService.GetConfig<List<string>>(ConfigsListKey);

            foreach (var configKey in availableConfigs)
            {
                var config = _remoteConfigService.GetConfig<VfxConfig>(configKey);

                _particlePrefabAddresses.Add(config.VfxType, config.ParticlePrefabAddress);
            }

            return UniTask.CompletedTask;
        }

        public async UniTask<IEnumerable<Particle>> Create(VfxType vfxType,Transform parent, int count)
        {
            var prefab = await _assetProvider.Load<GameObject>(_particlePrefabAddresses[vfxType]);

            var particles = new List<Particle>();

            for (int i = 0; i < count; i++)
                particles.Add(_instantiator.InstantiatePrefabForComponent<Particle>(prefab, parent));

            return particles;
        }
    }
}
