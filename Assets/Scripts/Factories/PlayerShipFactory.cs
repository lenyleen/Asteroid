using System;
using System.Collections.Generic;
using DataObjects;
using Installers;
using Interfaces;
using Player;
using Services;
using UnityEngine;
using Weapon;
using Zenject;

namespace Factories
{
    public class PlayerShipFactory
    {
        private readonly IInstantiator _instantiator;
        
        private readonly ShipInstaller.PlayerInstallData _playerInstallData;
        private readonly IFactory<ProjectileType, WeaponData, string,IWeaponsHolder, WeaponViewModel>  _weaponFactory;
        private readonly PlayerInputController _playerInputController;
        private readonly IPlayerPositionProvider _playerDataProvider;
        private readonly IPlayerWeaponInfoProviderService _playerWeaponInfoProviderService;

        public PlayerShipFactory(ShipInstaller.PlayerInstallData playerInstallData,
            IFactory<ProjectileType, WeaponData,string, IWeaponsHolder, WeaponViewModel> weaponFactory,
            PlayerInputController playerInputController, DiContainer instantiator, IPlayerPositionProvider playerDataProvider,
            IPlayerWeaponInfoProviderService playerWeaponInfoProviderService)
        {
            _playerInstallData = playerInstallData;
            _weaponFactory = weaponFactory;
            _playerInputController = playerInputController;
            _playerDataProvider = playerDataProvider;
            _playerWeaponInfoProviderService = playerWeaponInfoProviderService;
            _instantiator = instantiator;
        }
        
        public ShipViewModel SpawnShip()
        {
            var shipModel = _instantiator.Instantiate<ShipModel>(new object[]{_playerInstallData.ShipPreferences});
            
            var shipViewModel = _instantiator.Instantiate<ShipViewModel>(new object[]{shipModel});
            shipViewModel.Initiialize();
            
            _playerDataProvider.ApplyPlayer(shipViewModel);
            
            var shipView = _instantiator.InstantiatePrefabForComponent<Player.Ship>(
                _playerInstallData.ShipPrefab,
                _playerInstallData.PlayerSpawnPosition.position, 
                Quaternion.identity, 
                null
            );
            
            SpawnPlayerWeapons(shipView.PlayerWeapons);
            
            shipView.Construct(shipViewModel);
            
            return shipViewModel;
        }


        private void SpawnPlayerWeapons(PlayerWeapons playerWeapons)
        {
            var heavyWeapons = new List<WeaponViewModel>();
            var mainWeapons = new List<WeaponViewModel>();

            var heavyWeaponIndex = 1; 
            
            foreach (var weaponData in _playerInstallData.PlayerHeavyWeaponsData)
            {
                var name = $"{weaponData.Type} {heavyWeaponIndex}";
                var newWeapon = _weaponFactory.Create(weaponData.ProjectileType, weaponData,name, playerWeapons);
                heavyWeapons.Add(newWeapon);
                _playerWeaponInfoProviderService.ApplyWeaponInfoProvider(newWeapon);

                heavyWeaponIndex++;
            }

            foreach (var weaponData in _playerInstallData.PlayerMainWeaponsData)  
            {
                var newWeapon = _weaponFactory.Create(weaponData.ProjectileType, weaponData, weaponData.Type.ToString(), playerWeapons);
                mainWeapons.Add(newWeapon);
            }
            
            var weaponsViewModel = _instantiator.Instantiate<PlayerWeaponsViewModel>(new object[] { mainWeapons,heavyWeapons, _playerInputController,
                _playerDataProvider.PositionProvider.Value});
            
            
            weaponsViewModel.Initialize();
            playerWeapons.Initialize(weaponsViewModel);
        }
    }
}