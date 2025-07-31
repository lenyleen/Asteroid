using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Configs;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Projectiles;
using UnityEngine;

namespace _Project.Scripts.Factories
{
    public class ProjectileFactory
    {
        private readonly Projectile.Pool _pool;
        private readonly Dictionary<ProjectileType, ProjectileConfig> _projectileDatas;

        public ProjectileFactory(Projectile.Pool pool, List<ProjectileConfig> projectileDatas)
        {
            _pool = pool;

            _projectileDatas = projectileDatas.ToDictionary(data => data.Type, data => data);
        }

        public void Create(ProjectileType type, Vector3 spawnPosition,
            IPositionProvider positionProvider)
        {
            if (!_projectileDatas.TryGetValue(type, out var data))
                throw new Exception($"ProjectileData not found for type: {type}");

            var behaviour = CreateBehaviour(type, positionProvider);

            var projectileInitData = new ProjectileInitData(data,behaviour, spawnPosition,
                positionProvider.Rotation.Value, positionProvider.Velocity.Value);

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
