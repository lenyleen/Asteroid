using DataObjects;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Weapon
{
    public abstract class WeaponProjectileCreatorBase : IWeaponProjectileCreator
    {
        protected readonly IFactory<ProjectileType,Vector2,IProjectileTarget, IProjectileBehaviour> _projectileBehaviourFactory;
        protected readonly IFactory<ProjectileType,float,FireData,IProjectile>  _projectileFactory;

        protected WeaponProjectileCreatorBase(IFactory<ProjectileType, Vector2, IProjectileTarget, IProjectileBehaviour> projectileBehaviourFactory,
            IFactory<ProjectileType,float,FireData,IProjectile> projectileFactory)
        {
            _projectileBehaviourFactory = projectileBehaviourFactory;
            _projectileFactory = projectileFactory;
        }

        public abstract IProjectile CreateProjectile(Vector3 position, float rotation);
    }
}