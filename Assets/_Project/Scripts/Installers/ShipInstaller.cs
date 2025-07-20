using System;
using System.Collections.Generic;
using DataObjects;
using Factories;
using Handlers;
using Player;
using Projectiles;
using Services;
using UnityEngine;
using UnityEngine.Serialization;
using Weapon;
using Zenject;

namespace Installers
{
    public class ShipInstaller : MonoInstaller<ShipInstaller>
    {
        [SerializeField]private List<ProjectileData> _projectileDatas;
        [SerializeField]private WeaponView _weaponViewPrefab;
        [SerializeField]private Projectile _projectilePrefab;
        [SerializeField]private PlayerInstallData _playerInstallData;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameEvenstService>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<PlayerDataProviderService>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerWeaponsInfoProviderService>().AsSingle();
            Container.BindMemoryPool<Projectile, Projectile.Pool>().WithInitialSize(20).FromComponentInNewPrefab(_projectilePrefab).UnderTransformGroup("Projectiles");
            Container.BindInterfacesAndSelfTo<ProjectileFactory>().AsSingle().WithArguments(_projectileDatas);
            Container.BindInterfacesAndSelfTo<WeaponFactory>().AsSingle().WithArguments(_weaponViewPrefab);

            Container.BindInterfacesAndSelfTo<PlayerShipFactory>().AsSingle().WithArguments(_playerInstallData);
            Container.BindInterfacesAndSelfTo<ShipSpawnService>().AsSingle();

            Container.BindInterfacesAndSelfTo<StartGameByInputHandler>().AsSingle();
        }
        
        [Serializable]
        public class PlayerInstallData
        {
            [field: FormerlySerializedAs("<PlayerPrefab>k__BackingField")] [field: SerializeField] public Player.Ship ShipPrefab { get; private set; }
            [field: SerializeField] public Transform PlayerSpawnPosition { get;  private set; } 
            [field: FormerlySerializedAs("<PlayerPreferences>k__BackingField")] [field: SerializeField] public ShipPreferences ShipPreferences { get; private set; }
            [field: SerializeField] public List<WeaponData> PlayerHeavyWeaponsData { get; private set;}
            [field: SerializeField] public List<WeaponData> PlayerMainWeaponsData { get; private set; }
        }
    }
}