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
    public class ProjectileFactory : IFactory<ProjectileType,IPositionProvider,ProjectileViewModel>
    {
        private readonly IInstantiator _instantiator;
        private readonly Dictionary<ProjectileType, ProjectileData> _projectileDatas;
        private readonly Projectile.Pool _pool;
        
        public ProjectileFactory(Projectile.Pool pool ,List<ProjectileData> projectileDatas, IInstantiator instantiator)
        {
            _pool = pool;
            _instantiator = instantiator;
            _projectileDatas = projectileDatas.ToDictionary(data => data.Type, data => data );
        }
        
        public ProjectileViewModel Create(ProjectileType type, IPositionProvider positionProvider)
        {
            if (!_projectileDatas.TryGetValue(type, out var data))
                throw new Exception($"ProjectileData not found for type: {type}");

            var behaviour = CreateBehaviour(type, positionProvider);
            
            var projectileModel = _instantiator.Instantiate<ProjectileModel>(new object[] {data, behaviour, 
                positionProvider.Position.Value, positionProvider.Rotation.Value, positionProvider.Velocity.Value});
            
            var projectileViewMode = _instantiator.Instantiate<ProjectileViewModel>(new object[]{projectileModel});

            var projectile = _pool.Spawn(data.Sprite,projectileViewMode, prj => _pool.Despawn(prj));
            
            return projectileViewMode;
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