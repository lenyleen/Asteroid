using System;
using System.Collections.Generic;
using DataObjects;
using Interfaces;
using Projectiles;
using UnityEngine;
using Zenject;

namespace Factories
{
    public class ProjectileFactory : IFactory<ProjectileType,Vector2,IProjectileTarget,IProjectile>
    {
        private readonly Dictionary<ProjectileType,ProjectileData> _projectileDatas;
        private readonly Projectile.Pool _pool;
        
        public IProjectile Create(ProjectileType type, Vector2 direction, IProjectileTarget target = null)
        {
            if (!_projectileDatas.TryGetValue(type, out var data))
                throw new Exception($"ProjectileData not found for type: {type}");
            
            var behaviour = CreateProjectileBehaviour(type, direction, data,target);
            var projectile = _pool.Spawn(data.Sprite, prj => _pool.Despawn(prj));
            return projectile;
        }

        private IProjectileBehaviour CreateProjectileBehaviour(ProjectileType type, Vector2 direction, 
            ProjectileData data, IProjectileTarget target = null)
        {
            return type switch
            {
                ProjectileType.Bullet => new BulletBehaviour(direction, data.Speed, data.LifetimeInSeconds),
                ProjectileType.Laser => new LaserBehaviour(direction, data.LifetimeInSeconds),
                _ => throw new NotImplementedException()
            };
        }
        
    }
}