using System;
using System.Collections.Generic;
using _Project.Scripts.Configs;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Projectiles;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Factories
{
    public class ProjectileFactory : IInitializable
    {
        private const string ProjectilesRemoteConfigsKeys = "ActiveProjectilesKeys";

        private readonly Projectile.Pool _pool;
        private readonly IScenesAssetProvider _assetProvider;
        private readonly IRemoteConfigService _remoteConfigService;

        private Dictionary<ProjectileType, ProjectileConfig> _projectileDatas = new ();

        public ProjectileFactory(Projectile.Pool pool, IScenesAssetProvider assetProvider,
            IRemoteConfigService remoteConfigService)
        {
            _pool = pool;
            _assetProvider = assetProvider;
            _remoteConfigService = remoteConfigService;
        }

        public void Initialize()
        {
            var projectilesKeys = _remoteConfigService.GetConfig<List<string>>(ProjectilesRemoteConfigsKeys);

            foreach (var projectileKey in projectilesKeys)
            {
                var config = _remoteConfigService.GetConfig<ProjectileConfig>(projectileKey);
                _projectileDatas.Add(config.Type, config);
            }
        }

        public async UniTask Create(ProjectileType type, Vector3 spawnPosition,
            IPositionProvider positionProvider)
        {
            if (!_projectileDatas.TryGetValue(type, out var data))
                throw new Exception($"ProjectileData not found for type: {type}");

            var behaviour = CreateBehaviour(type, positionProvider);

            var sprite = await _assetProvider.Load<Sprite>(data.SpriteAddress);

            var projectileInitData = new ProjectileInitData(sprite, behaviour, spawnPosition,
                positionProvider.Rotation.Value, positionProvider.Velocity.Value, data.ColliderConfig,
                data.LifetimeInSeconds);

            _pool.Spawn(projectileInitData, prj => _pool.Despawn(prj));
        }

        private IProjectileBehaviour CreateBehaviour(ProjectileType type, IPositionProvider positionProvider)
        {
            if (!_projectileDatas.TryGetValue(type, out var data))
                throw new Exception($"ProjectileData not found for type: {type}");

            return type switch
            {
                ProjectileType.Laser => new LaserBehaviour(positionProvider),
                ProjectileType.Bullet => new BulletBehaviour(data.Speed),
                _ => new BulletBehaviour(data.Speed)
            };
        }
    }
}
