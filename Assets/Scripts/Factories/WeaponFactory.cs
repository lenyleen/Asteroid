using System.Collections.Generic;
using System.Linq;
using DataObjects;
using Interfaces;
using UnityEngine;
using Weapon;
using Zenject;

namespace Factories
{
    public class WeaponFactory : IFactory<ProjectileType, WeaponData, IWeaponsHolder, WeaponViewModel>
    {
        private readonly DiContainer _diContainer;
        private readonly IFactory<ProjectileType, Vector2, IProjectileTarget, IProjectileBehaviour>
            _projectileBehaviourFactory;
        private readonly IFactory<ProjectileType,float,FireData,IProjectile> _projectileFactory;
        private readonly WeaponView _weaponViewPrefab;


        public WeaponFactory(IFactory<ProjectileType, Vector2, IProjectileTarget, IProjectileBehaviour> projectileBehaviourFactory, 
            IFactory<ProjectileType,float,FireData,IProjectile> projectileFactory, DiContainer diContainer, WeaponView weaponViewPrefab)
        {
            _diContainer = diContainer;
            _projectileBehaviourFactory = projectileBehaviourFactory;
            _projectileFactory = projectileFactory;
            _weaponViewPrefab = weaponViewPrefab;
        }

        public WeaponViewModel Create(ProjectileType type, WeaponData data, IWeaponsHolder weaponsHolder)
        {
            var model = new WeaponModel(data);
            var viewModel = new WeaponViewModel(CreateProjectileCreator(type),model);
            viewModel.Initialize();
            _diContainer.Resolve<TickableManager>().AddFixed(viewModel);
            var view = _diContainer.InstantiatePrefabForComponent<WeaponView>(_weaponViewPrefab);
            view.Initialize(viewModel,data.Sprite);
            
            weaponsHolder.ApplyWeapons(data.Type,view);
            
            return viewModel;
        }

        private IWeaponProjectileCreator CreateProjectileCreator(ProjectileType projectileType)
        {
            return projectileType switch
            {
                ProjectileType.Laser => new LaserGunProjectileCreator(_projectileBehaviourFactory, _projectileFactory),
                ProjectileType.Bullet =>
                    new BulletGunProjectileCreator(_projectileBehaviourFactory, _projectileFactory),
                _ => throw new System.NotImplementedException($"Projectile type {projectileType} is not implemented.")
            };
        }
    }
}