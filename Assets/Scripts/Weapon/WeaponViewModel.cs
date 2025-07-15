using DataObjects;
using Interfaces;
using UniRx;
using UnityEngine;
using Zenject;

namespace Weapon
{
    public class WeaponViewModel
    {
        public ReactiveProperty<IProjectile> Projectile { get; } = new ReactiveProperty<IProjectile>();
        
        private readonly IFactory<ProjectileType,Vector2,IProjectileTarget,IProjectile>  _projectileFactory;

        public void Fire(Vector2 direction)
        {
            //if(_model.Cooldown)
            //return
            Projectile.Value = _projectileFactory.Create(//model.ProjectileType, direction,null)
        }
    }
}