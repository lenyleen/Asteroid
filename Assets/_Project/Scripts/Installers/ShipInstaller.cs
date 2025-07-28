using System;
using System.Collections.Generic;
using Configs;
using Factories;
using Handlers;
using Player;
using Projectiles;
using Services;
using UnityEngine;
using Weapon;
using Zenject;

namespace Installers
{
    public class ShipInstaller : MonoInstaller<ShipInstaller>
    {
        [SerializeField] private List<ProjectileConfig> _projectileDatas;
        [SerializeField] private WeaponView _weaponViewPrefab;
        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private PlayerInstallData _playerInstallData;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameEvenstService>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerDataProviderService>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerWeaponsInfoProviderService>()
                .AsSingle();

            Container.BindMemoryPool<Projectile, Projectile.Pool>()
                .WithInitialSize(20)
                .FromComponentInNewPrefab(_projectilePrefab).UnderTransformGroup("Projectiles");

            Container.BindInterfacesAndSelfTo<ProjectileFactory>()
                .AsSingle()
                .WithArguments(_projectileDatas);

            Container.BindInterfacesAndSelfTo<WeaponFactory>()
                .AsSingle()
                .WithArguments(_weaponViewPrefab);

            Container.BindInterfacesAndSelfTo<PlayerShipFactory>()
                .AsSingle()
                .WithArguments(_playerInstallData);

            Container.BindInterfacesAndSelfTo<ShipSpawnService>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<StartGameByInputHandler>()
                .AsSingle();
        }

        [Serializable]
        public class PlayerInstallData
        {
            [field: SerializeField] public Ship ShipPrefab { get; private set; }
            [field: SerializeField] public Transform PlayerSpawnPosition { get; private set; }
            [field: SerializeField] public ShipPreferences ShipPreferences { get; private set; }
            [field: SerializeField] public List<WeaponConfig> PlayerHeavyWeaponsData { get; private set; }
            [field: SerializeField] public List<WeaponConfig> PlayerMainWeaponsData { get; private set; }
        }
    }
}
