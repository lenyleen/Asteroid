using DataObjects;
using Interfaces;
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
            var view = _instantiator.InstantiatePrefabForComponent<WeaponView>(_weaponViewPrefab);
            var offsetFromHolder = weaponsHolder.ApplyWeapon(data.Type,view);
            
            var model = _instantiator.Instantiate<WeaponModel>(new object[]{data,name, offsetFromHolder});
            var viewModel = _instantiator.Instantiate<WeaponViewModel>(new object[]{model});
            viewModel.Initialize();
            
            
            view.Initialize(viewModel,data.Sprite);
            
            return viewModel;
        }
    }
}