using System;
using System.Collections.Generic;
using Configs;
using Factories;
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
            Container.BindInterfacesAndSelfTo<ShipStateProviderService>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerWeaponsInfoProviderService>()
                .AsSingle();

            Container.BindMemoryPool<Projectile, Projectile.Pool>()
                .WithInitialSize(20)
                .FromComponentInNewPrefab(_projectilePrefab).UnderTransformGroup("Projectiles");

            Container.Bind<ProjectileFactory>()
                .AsSingle()
                .WithArguments(_projectileDatas);

            Container.Bind<WeaponFactory>()
                .AsSingle()
                .WithArguments(_weaponViewPrefab);

            Container.BindInterfacesAndSelfTo<PlayerShipFactory>()
                .AsSingle()
                .WithArguments(_playerInstallData);
        }

        [Serializable]
        public class PlayerInstallData
        {
            [field: SerializeField] public Ship ShipPrefab { get; private set; }
            [field: SerializeField] public Transform PlayerSpawnPosition { get; private set; }
            [field: SerializeField] public ShipPreferences ShipPreferences { get; private set; }
            [field: SerializeField] public WeaponConfig PlayerHeavyWeaponsData { get; private set; }
            [field: SerializeField] public WeaponConfig PlayerMainWeaponsData { get; private set; }
        }
    }
}
