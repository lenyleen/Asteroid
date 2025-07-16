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
    public class PlayerSpawner
    {
        private readonly DiContainer _container;
        
        private readonly PlayerInstaller.PlayerInstallData _playerInstallData;
        private readonly IFactory<ProjectileType, WeaponData, IWeaponsHolder, WeaponViewModel>  _weaponFactory;
        private readonly PlayerInputController _playerInputController;
        private readonly IPlayerPositionProvider _playerDataProvider;

        public PlayerSpawner(PlayerInstaller.PlayerInstallData playerInstallData,
            IFactory<ProjectileType, WeaponData, IWeaponsHolder, WeaponViewModel> weaponFactory,
            PlayerInputController playerInputController, DiContainer container, IPlayerPositionProvider playerDataProvider)
        {
            _playerInstallData = playerInstallData;
            _weaponFactory = weaponFactory;
            _playerInputController = playerInputController;
            _playerDataProvider = playerDataProvider;
            _container = container;
        }
        
        public void SpawnPlayer()
        {
            var playerModel = new PlayerModel(_playerInstallData.PlayerPreferences);
            _container.BindInterfacesAndSelfTo<PlayerModel>()
                .FromInstance(playerModel)
                .AsSingle();

            var playerViewModel = _container.Instantiate<PlayerViewModel>();
            _container.Bind<IFixedTickable>().To<PlayerViewModel>().FromInstance(playerViewModel);
            _container.Resolve<TickableManager>().AddFixed(playerViewModel);
            
            _playerDataProvider.ApplyPlayer(playerViewModel);
            
            var player = _container.InstantiatePrefabForComponent<Player.Player>(
                _playerInstallData.PlayerPrefab,
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
            }

            foreach (var weaponData in _playerInstallData.PlayerMainWeaponsData)  
            {
                var newWeapon = _weaponFactory.Create(weaponData.ProjectileType, weaponData, playerWeapons);
                mainWeapons.Add(newWeapon);
            }
            
            var weaponsViewModel = new PlayerWeaponsViewModel(mainWeapons,heavyWeapons, _playerInputController
                , _playerDataProvider.PositionProvider);
            
            _container.BindInterfacesAndSelfTo<PlayerWeaponsViewModel>()
                .FromInstance(weaponsViewModel)
                .AsSingle();
            _container.Resolve<TickableManager>().AddFixed(weaponsViewModel);
        }
        
        
    }
}