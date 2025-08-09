using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Configs;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Projectiles;
using Cysharp.Threading.Tasks;
using Services;
using UnityEngine;

namespace Factories
{
    public class ProjectileFactory
    {
        private readonly Projectile.Pool _pool;
        private readonly Dictionary<ProjectileType, ProjectileConfig> _projectileDatas;
        private readonly AssetProvider _assetProvider;

        public ProjectileFactory(Projectile.Pool pool, List<ProjectileConfig> projectileDatas,
            AssetProvider assetProvider)
        {
            _pool = pool;
            _assetProvider = assetProvider;

            _projectileDatas = projectileDatas.ToDictionary(data => data.Type, data => data);
        }

        public async UniTask Create(ProjectileType type, Vector3 spawnPosition,
            IPositionProvider positionProvider)
        {
            if (!_projectileDatas.TryGetValue(type, out var data))
                throw new Exception($"ProjectileData not found for type: {type}");

            var behaviour = CreateBehaviour(type, positionProvider);

            var sprite = await _assetProvider.Load<Sprite>(data.Sprite);

            var projectileInitData = new ProjectileInitData(sprite,behaviour, spawnPosition,
                positionProvider.Rotation.Value, positionProvider.Velocity.Value, data.ColliderConfig, data.LifetimeInSeconds);

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
