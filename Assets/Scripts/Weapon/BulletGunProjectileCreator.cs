using DataObjects;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Weapon
{
    public class BulletGunProjectileCreator : WeaponProjectileCreatorBase
    {
        public BulletGunProjectileCreator(
            IFactory<ProjectileType, Vector2, IProjectileTarget, IProjectileBehaviour> projectileBehaviourFactory,
            IFactory<ProjectileType, float, FireData, IProjectile> projectileFactory) : base(projectileBehaviourFactory,
            projectileFactory)
        {
        }

        public override IProjectile CreateProjectile(Vector3 position,float rotation)
        {
            var angle = rotation * Mathf.Deg2Rad;
            var direction = new Vector2(Mathf.Sin(-angle), Mathf.Cos(angle));
            var fireData = new FireData(_projectileBehaviourFactory.Create(ProjectileType.Bullet, direction, null));
            return _projectileFactory.Create(ProjectileType.Bullet,rotation, fireData);
        }
    }
}