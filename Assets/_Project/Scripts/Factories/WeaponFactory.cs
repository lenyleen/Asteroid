using System.Collections.Generic;
using _Project.Scripts.Configs;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Weapon;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.Factories
{
    public class WeaponFactory : ISceneInitializable
    {
        private const string ActiveRemoteWeaponsConfigKey = "ActiveWeaponKeys";

        private readonly IInstantiator _instantiator;
        private readonly AssetReference _weaponViewPrefabReference;
        private readonly IScenesAssetProvider _assetProvider;
        private readonly IRemoteConfigService _remoteConfigService;
        private readonly Dictionary<WeaponType, WeaponConfig> _configs = new();

        private WeaponView _prefab;

        public WeaponFactory(DiContainer instantiator, AssetReference weaponViewPrefabReference,
            IScenesAssetProvider assetProvider, IRemoteConfigService remoteConfigService)
        {
            _instantiator = instantiator;
            _weaponViewPrefabReference = weaponViewPrefabReference;
            _assetProvider = assetProvider;
            _remoteConfigService = remoteConfigService;
        }

        public async UniTask InitializeAsync()
        {
            var activeWeaponsKeys = _remoteConfigService.GetConfig<List<string>>(ActiveRemoteWeaponsConfigKey);

            foreach (var activeWeaponsKey in activeWeaponsKeys)
            {
                var config = _remoteConfigService.GetConfig<WeaponConfig>(activeWeaponsKey);
                _configs.Add(config.Type, config);
            }

            _prefab = (await _assetProvider.Load<GameObject>(_weaponViewPrefabReference))
                .GetComponent<WeaponView>();
        }

        public async UniTask<WeaponViewModel> Create(WeaponType type, string name,
            IWeaponsHolder weaponsHolder, Vector3 position)
        {
            var config = _configs[type];

            var model = _instantiator.Instantiate<WeaponModel>(new object[] { config, name, position });
            var viewModel = _instantiator.Instantiate<WeaponViewModel>(new object[] { model });
            viewModel.Initialize();

            var sprite = await _assetProvider.Load<Sprite>(config.SpriteAddress);
            var audio = await _assetProvider.Load<AudioClip>(config.WeaponSoundAddress);

            var view = _instantiator.InstantiatePrefabForComponent<WeaponView>(_prefab, new object[]{config.VFXType, audio});
            weaponsHolder.ApplyWeapon(config.Type, view, position);

            view.Initialize(viewModel, sprite);

            return viewModel;
        }
    }
}
