using System.Collections.Generic;
using System.Linq;
using DataObjects;
using Interfaces;
using Projectiles;
using UnityEngine;
using Weapon;
using Zenject;

namespace Factories
{
    public class WeaponFactory : IFactory<ProjectileType, WeaponData,string, IWeaponsHolder, WeaponViewModel>
    {
        private readonly IInstantiator _instantiator;
        private readonly WeaponView _weaponViewPrefab;


        public WeaponFactory(DiContainer instantiator, WeaponView weaponViewPrefab)
        {
            _instantiator = instantiator;
            _weaponViewPrefab = weaponViewPrefab;
        }

        public WeaponViewModel Create(ProjectileType type,WeaponData data,string name, IWeaponsHolder weaponsHolder)
        {
            var model = _instantiator.Instantiate<WeaponModel>(new object[]{data,name});
            var viewModel = _instantiator.Instantiate<WeaponViewModel>(new object[]{model});
            viewModel.Initialize();
            
            var view = _instantiator.InstantiatePrefabForComponent<WeaponView>(_weaponViewPrefab);
            view.Initialize(viewModel,data.Sprite);
            
            weaponsHolder.ApplyWeapons(data.Type,view);
            
            return viewModel;
        }
    }
}