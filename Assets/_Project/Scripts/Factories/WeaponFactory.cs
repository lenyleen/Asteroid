using _Project.Scripts.Configs;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Weapon;
using Configs;
using Cysharp.Threading.Tasks;
using Interfaces;
using Services;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Weapon;
using Zenject;

namespace Factories
{
    public class WeaponFactory : IAsyncInitializable
    {
        private readonly IInstantiator _instantiator;
        private readonly AssetReference _weaponViewPrefabReference;
        private readonly AssetProvider _assetProvider;

        private WeaponView _prefab;

        public WeaponFactory(DiContainer instantiator, AssetReference weaponViewPrefabReference, AssetProvider assetProvider)
        {
            _instantiator = instantiator;
            _weaponViewPrefabReference = weaponViewPrefabReference;
            _assetProvider = assetProvider;
        }

        public async UniTask InitializeAsync()
        {
            _prefab = await _assetProvider.Load<WeaponView>(_weaponViewPrefabReference);
        }

        public async UniTask<WeaponViewModel> Create(WeaponConfig config, string name,
            IWeaponsHolder weaponsHolder, Vector3 position)
        {
            var view = _instantiator.InstantiatePrefabForComponent<WeaponView>(_prefab);
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
