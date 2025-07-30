using System.Collections.Generic;
using Configs;
using Installers;
using Interfaces;
using Player;
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
        private readonly IFactory<ProjectileType, WeaponConfig, string, IWeaponsHolder, WeaponViewModel> _weaponFactory;

        private ShipViewModel _shipViewModel;

        public PlayerShipFactory(ShipInstaller.PlayerInstallData playerInstallData,
            IFactory<ProjectileType, WeaponConfig, string, IWeaponsHolder, WeaponViewModel> weaponFactory,
            PlayerInputController playerInputController, DiContainer instantiator,
            IPlayerStateProviderService playerDataProviderService,
            IPlayerWeaponInfoProviderService playerWeaponInfoProviderService)
        {
            _playerInstallData = playerInstallData;
            _weaponFactory = weaponFactory;
            _playerInputController = playerInputController;
            _playerDataProviderService = playerDataProviderService;
            _playerWeaponInfoProviderService = playerWeaponInfoProviderService;
            _instantiator = instantiator;
        }

        public void SpawnShip()
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

            SpawnPlayerWeapons(shipView.PlayerWeapons);

            shipView.Initialize(_shipViewModel);
        }

        private void SpawnPlayerWeapons(PlayerWeapons playerWeapons)
        {
            var heavyWeapons = new List<WeaponViewModel>();
            var mainWeapons = new List<WeaponViewModel>();

            var heavyWeaponIndex = 1;

            foreach (var weaponData in _playerInstallData.PlayerHeavyWeaponsData)
            {
                var name = $"{weaponData.Type} {heavyWeaponIndex}";
                var newWeapon = _weaponFactory.Create(weaponData.ProjectileType, weaponData, name, playerWeapons);
                heavyWeapons.Add(newWeapon);
                _playerWeaponInfoProviderService.ApplyWeaponInfoProvider(newWeapon);

                heavyWeaponIndex++;
            }

            foreach (var weaponData in _playerInstallData.PlayerMainWeaponsData)
            {
                var newWeapon = _weaponFactory.Create(weaponData.ProjectileType, weaponData, weaponData.Type.ToString(),
                    playerWeapons);
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
