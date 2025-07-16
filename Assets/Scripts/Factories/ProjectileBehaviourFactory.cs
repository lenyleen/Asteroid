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
    public class ProjectileBehaviourFactory : IFactory<ProjectileType,Vector2,IProjectileTarget, IProjectileBehaviour>
    {
        private readonly Dictionary<ProjectileType,ProjectileData> _projectileDatas;

        public ProjectileBehaviourFactory(List<ProjectileData> projectileDatas)
        {
            _projectileDatas = projectileDatas.ToDictionary(data => data.Type, data => data );
            
        }
        public IProjectileBehaviour Create(ProjectileType type,Vector2 direction, IProjectileTarget target = null)
        {
            if (!_projectileDatas.TryGetValue(type, out var data))
                throw new Exception($"ProjectileData not found for type: {type}");
            
            return type switch
            {
                ProjectileType.Laser => new LaserBehaviour(direction,data.LifetimeInSeconds),
                ProjectileType.Bullet => new BulletBehaviour(direction,data.Speed,data.LifetimeInSeconds),
                _ => new BulletBehaviour(direction,data.Speed, data.LifetimeInSeconds)
            };
        }
    }

}