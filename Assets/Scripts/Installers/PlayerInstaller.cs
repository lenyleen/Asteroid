using System;
using System.Collections.Generic;
using DataObjects;
using Factories;
using Player;
using Projectiles;
using Services;
using UnityEngine;
using UnityEngine.Serialization;
using Weapon;
using Zenject;

namespace Installers
{
    public class PlayerInstaller : Installer<PlayerInstaller.PlayerInstallData,Projectile,List<ProjectileData>,WeaponView,PlayerInstaller>
    {
        private List<ProjectileData> _projectileDatas;
        private WeaponView _weaponViewPrefab;
        private Projectile _projectilePrefab;
        private PlayerInstallData _playerInstallData;
        public PlayerInstaller(PlayerInstallData playerInstallData,Projectile projectile,List<ProjectileData> projectileDatas, WeaponView weaponViewPrefab)
        {
            _playerInstallData = playerInstallData;
            _projectileDatas = projectileDatas;
            _weaponViewPrefab = weaponViewPrefab;
            _projectilePrefab = projectile;
        }

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PlayerDataProviderService>().AsSingle();
            Container.BindMemoryPool<Projectile, Projectile.Pool>().WithInitialSize(20).FromComponentInNewPrefab(_projectilePrefab).UnderTransformGroup("Projectiles");
            Container.BindInterfacesAndSelfTo<ProjectileFactory>().AsSingle().WithArguments(_projectileDatas);
            Container.BindInterfacesAndSelfTo<ProjectileBehaviourFactory>().AsSingle().WithArguments(_projectileDatas);
            Container.BindInterfacesAndSelfTo<WeaponFactory>().AsSingle().WithArguments(_weaponViewPrefab);

            Container.BindInterfacesAndSelfTo<PlayerSpawner>().AsSingle().WithArguments(_playerInstallData);


            Container.BindInterfacesAndSelfTo<GameService>().AsSingle();
        }
        
        [Serializable]
        public class PlayerInstallData
        {
            [field: SerializeField] public Player.Player PlayerPrefab { get; private set; }
            [field: SerializeField] public Transform PlayerSpawnPosition { get;  private set; } 
            [field: SerializeField] public PlayerPreferences PlayerPreferences { get; private set; }
            [field: SerializeField] public List<WeaponData> PlayerHeavyWeaponsData { get; private set;}
            [field: SerializeField] public List<WeaponData> PlayerMainWeaponsData { get; private set; }
        }
    }
}