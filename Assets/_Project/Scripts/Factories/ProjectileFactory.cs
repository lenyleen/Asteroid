using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Configs;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Projectiles;
using _Project.Scripts.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Factories
{
    public class ProjectileFactory : IAsyncInitializable
    {
        private const string ProjectileConfigLabel = "ProjectileConfigs";

        private readonly Projectile.Pool _pool;
        private readonly AssetProvider _assetProvider;

        private Dictionary<ProjectileType, ProjectileConfig> _projectileDatas;

        public ProjectileFactory(Projectile.Pool pool, AssetProvider assetProvider)
        {
            _pool = pool;
            _assetProvider = assetProvider;
        }

        public async UniTask InitializeAsync()
        {
            var projectileConfigs =
                await _assetProvider.LoadMany<ProjectileConfig>(ProjectileConfigLabel);

            _projectileDatas = projectileConfigs.ToDictionary(data => data.Type, data => data);
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
