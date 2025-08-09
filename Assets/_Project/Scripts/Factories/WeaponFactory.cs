using Configs;
using Cysharp.Threading.Tasks;
using Interfaces;
using Services;
using UnityEngine;
using Weapon;
using Zenject;

namespace Factories
{
    public class WeaponFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly WeaponView _weaponViewPrefab;
        private readonly AssetProvider _assetProvider;

        public WeaponFactory(DiContainer instantiator, WeaponView weaponViewPrefab, AssetProvider assetProvider)
        {
            _instantiator = instantiator;
            _weaponViewPrefab = weaponViewPrefab;
            _assetProvider = assetProvider;
        }

        public async UniTask<WeaponViewModel> Create(WeaponConfig config, string name,
            IWeaponsHolder weaponsHolder, Vector3 position)
        {
            var view = _instantiator.InstantiatePrefabForComponent<WeaponView>(_weaponViewPrefab);
            var offsetFromHolder = weaponsHolder.ApplyWeapon(config.Type, view, position);

            var model = _instantiator.Instantiate<WeaponModel>(new object[] { config, name, offsetFromHolder });
            var viewModel = _instantiator.Instantiate<WeaponViewModel>(new object[] { model });
            viewModel.Initialize();

            var sprite = await _assetProvider.Load<Sprite>(config.Sprite);

            view.Initialize(viewModel, sprite);

            return viewModel;
        }
    }
}
