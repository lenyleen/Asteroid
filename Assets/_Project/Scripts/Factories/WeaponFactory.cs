using _Project.Scripts.Configs;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Weapon;
using Zenject;

namespace _Project.Scripts.Factories
{
    public class WeaponFactory : IFactory<ProjectileType, WeaponConfig, string, IWeaponsHolder, WeaponViewModel>
    {
        private readonly IInstantiator _instantiator;
        private readonly WeaponView _weaponViewPrefab;

        public WeaponFactory(DiContainer instantiator, WeaponView weaponViewPrefab)
        {
            _instantiator = instantiator;
            _weaponViewPrefab = weaponViewPrefab;
        }

        public WeaponViewModel Create(ProjectileType type, WeaponConfig config, string name,
            IWeaponsHolder weaponsHolder)
        {
            var view = _instantiator.InstantiatePrefabForComponent<WeaponView>(_weaponViewPrefab);
            var offsetFromHolder = weaponsHolder.ApplyWeapon(config.Type, view);

            var model = _instantiator.Instantiate<WeaponModel>(new object[] { config, name, offsetFromHolder });
            var viewModel = _instantiator.Instantiate<WeaponViewModel>(new object[] { model });
            viewModel.Initialize();

            view.Initialize(viewModel, config.Sprite);

            return viewModel;
        }
    }
}
