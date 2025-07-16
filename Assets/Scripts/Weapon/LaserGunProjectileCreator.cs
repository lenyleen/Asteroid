using DataObjects;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Weapon
{
    public class LaserGunProjectileCreator : WeaponProjectileCreatorBase
    {
        public LaserGunProjectileCreator(IFactory<ProjectileType, Vector2, IProjectileTarget, IProjectileBehaviour> projectileBehaviourFactory,
            IFactory<ProjectileType,float,FireData,IProjectile>projectileFactory) 
            : base(projectileBehaviourFactory, projectileFactory)
        {
            
        }
        
        public override IProjectile CreateProjectile(Vector3 position, float rotation)
        {
            var angle = rotation * Mathf.Deg2Rad;
            var direction = new Vector2(Mathf.Sin(-angle), Mathf.Cos(angle));
            var fireData = new FireData(_projectileBehaviourFactory.Create(ProjectileType.Laser, direction, null));
            return _projectileFactory.Create(ProjectileType.Laser,rotation
                , fireData);
        }
    }
}