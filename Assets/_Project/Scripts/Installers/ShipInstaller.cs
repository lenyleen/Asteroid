using System;
using System.Collections.Generic;
using _Project.Scripts.Configs;
using _Project.Scripts.Factories;
using _Project.Scripts.Player;
using _Project.Scripts.Projectiles;
using _Project.Scripts.Services;
using _Project.Scripts.Weapon;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Installers
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

            Container.BindInterfacesAndSelfTo<ProjectileFactory>()
                .AsSingle()
                .WithArguments(_projectileDatas);

            Container.BindInterfacesAndSelfTo<WeaponFactory>()
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
            [field: SerializeField] public List<WeaponConfig> PlayerHeavyWeaponsData { get; private set; }
            [field: SerializeField] public List<WeaponConfig> PlayerMainWeaponsData { get; private set; }
        }
    }
}
