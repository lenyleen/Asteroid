using System.Collections.Generic;
using Configs;
using Cysharp.Threading.Tasks;
using Installers;
using Interfaces;
using Player;
using Services;
using UnityEditor;
using UnityEngine;
using Weapon;
using Zenject;

namespace Factories
{
    public class PlayerShipFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly IPlayerStateProviderService _playerDataProviderService;
        private readonly PlayerInputController _playerInputController;
        private readonly ShipInstaller.PlayerInstallData _playerInstallData;
        private readonly IPlayerWeaponInfoProviderService _playerWeaponInfoProviderService;
        private readonly WeaponFactory _weaponFactory;
        private readonly ShipViewConfig _viewConfig;
        private readonly AssetProvider _assetProvider;

        private ShipViewModel _shipViewModel;

        public PlayerShipFactory(ShipInstaller.PlayerInstallData playerInstallData,
            WeaponFactory weaponFactory, PlayerInputController playerInputController, DiContainer instantiator,
            IPlayerStateProviderService playerDataProviderService, AssetProvider assetProvider,
            IPlayerWeaponInfoProviderService playerWeaponInfoProviderService)
        {
            _playerInstallData = playerInstallData;
            _weaponFactory = weaponFactory;
            _playerInputController = playerInputController;
            _playerDataProviderService = playerDataProviderService;
            _playerWeaponInfoProviderService = playerWeaponInfoProviderService;
            _assetProvider = assetProvider;
            _instantiator = instantiator;
        }

        public async void SpawnShip()
        {
            var shipModel = _instantiator.Instantiate<ShipModel>(new object[] { _playerInstallData.ShipPreferences });

            _shipViewModel = _instantiator.Instantiate<ShipViewModel>(new object[] { shipModel });

            _playerDataProviderService.ApplyPlayer(_shipViewModel);

            var shipView = _instantiator.InstantiatePrefabForComponent<Ship>(
                _playerInstallData.ShipPrefab,
                _playerInstallData.PlayerSpawnPosition.position,
                Quaternion.identity,
                null
            );

            var shipSprite = await _assetProvider.Load<Sprite>(_viewConfig.ShipSprite);

            await SpawnPlayerWeapons(shipView.PlayerWeapons);

            shipView.Initialize(_shipViewModel,shipSprite);
        }

        private async UniTask SpawnPlayerWeapons(PlayerWeapons playerWeapons)
        {
            var heavyWeapons = new List<WeaponViewModel>();
            var mainWeapons = new List<WeaponViewModel>();

            var heavyWeaponIndex = 1;

            foreach (var heavyWeaponSlot in _viewConfig.HeavyWeaponSlots)
            {
                var name = $"{_playerInstallData.PlayerHeavyWeaponsData.Type} {heavyWeaponIndex}";
                var newWeapon = await _weaponFactory.Create(_playerInstallData.PlayerHeavyWeaponsData, name,
                    playerWeapons, heavyWeaponSlot);

                heavyWeapons.Add(newWeapon);
                _playerWeaponInfoProviderService.ApplyWeaponInfoProvider(newWeapon);

                heavyWeaponIndex++;
            }

            foreach (var lightWeaponSlot in _viewConfig.LightWeaponSlots)
            {
                var newWeapon = await _weaponFactory.Create(_playerInstallData.PlayerMainWeaponsData,
                    _playerInstallData.PlayerMainWeaponsData.Type.ToString(), playerWeapons, lightWeaponSlot);

                mainWeapons.Add(newWeapon);
            }

            var weaponsViewModel = _instantiator.Instantiate<PlayerWeaponsViewModel>(new object[]
            {
                mainWeapons, heavyWeapons, _playerInputController,
                _playerDataProviderService.PositionProvider.Value
            });

            weaponsViewModel.Initialize();
            playerWeapons.Initialize(weaponsViewModel);
        }
    }
}
