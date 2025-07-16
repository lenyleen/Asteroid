using System.Collections.Generic;
using DataObjects;
using Installers;
using Interfaces;
using Player;
using UnityEngine;
using Weapon;
using Zenject;

namespace Factories
{
    public class ShipSpawner
    {
        private readonly DiContainer _container;
        
        private readonly ShipInstaller.PlayerInstallData _playerInstallData;
        private readonly IFactory<ProjectileType, WeaponData, IWeaponsHolder, WeaponViewModel>  _weaponFactory;
        private readonly PlayerInputController _playerInputController;
        private readonly IPlayerPositionProvider _playerDataProvider;
        private readonly IPlayerWeaponInfoProviderService _playerWeaponInfoProviderService;

        public ShipSpawner(ShipInstaller.PlayerInstallData playerInstallData,
            IFactory<ProjectileType, WeaponData, IWeaponsHolder, WeaponViewModel> weaponFactory,
            PlayerInputController playerInputController, DiContainer container, IPlayerPositionProvider playerDataProvider,
            IPlayerWeaponInfoProviderService playerWeaponInfoProviderService)
        {
            _playerInstallData = playerInstallData;
            _weaponFactory = weaponFactory;
            _playerInputController = playerInputController;
            _playerDataProvider = playerDataProvider;
            _playerWeaponInfoProviderService = playerWeaponInfoProviderService;
            _container = container;
        }
        
        public void SpawnPlayer()
        {
            var playerModel = new ShipModel(_playerInstallData.PlayerPreferences);
            _container.BindInterfacesAndSelfTo<ShipModel>()
                .FromInstance(playerModel)
                .AsSingle();

            var playerViewModel = _container.Instantiate<ShipViewModel>();
            _container.Bind<IFixedTickable>().To<ShipViewModel>().FromInstance(playerViewModel);
            _container.Resolve<TickableManager>().AddFixed(playerViewModel);
            
            _playerDataProvider.ApplyPlayer(playerViewModel);
            
            var player = _container.InstantiatePrefabForComponent<Player.Ship>(
                _playerInstallData.ShipPrefab,
                _playerInstallData.PlayerSpawnPosition.position, 
                Quaternion.identity, 
                null
            );
            
            SpawnPlayerWeapons(player.PlayerWeapons);
            
            player.Construct(playerViewModel);
        }


        private void SpawnPlayerWeapons(PlayerWeapons playerWeapons)
        {
            var heavyWeapons = new List<WeaponViewModel>();
            var mainWeapons = new List<WeaponViewModel>();
            
            foreach (var weaponData in _playerInstallData.PlayerHeavyWeaponsData)
            {
                var newWeapon = _weaponFactory.Create(weaponData.ProjectileType, weaponData, playerWeapons);
                heavyWeapons.Add(newWeapon);
                _playerWeaponInfoProviderService.ApplyWeaponInfoProvider(newWeapon);
            }

            foreach (var weaponData in _playerInstallData.PlayerMainWeaponsData)  
            {
                var newWeapon = _weaponFactory.Create(weaponData.ProjectileType, weaponData, playerWeapons);
                mainWeapons.Add(newWeapon);
            }
            
            var weaponsViewModel = new PlayerWeaponsViewModel(mainWeapons,heavyWeapons, _playerInputController
                , _playerDataProvider.PositionProvider.Value);
            
            _container.BindInterfacesAndSelfTo<PlayerWeaponsViewModel>()
                .FromInstance(weaponsViewModel)
                .AsSingle();
            _container.Resolve<TickableManager>().AddFixed(weaponsViewModel);
        }
        
        
    }
}