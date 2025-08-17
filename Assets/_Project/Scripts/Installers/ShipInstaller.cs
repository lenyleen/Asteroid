using System;
using _Project.Scripts.Configs;
using _Project.Scripts.Factories;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Projectiles;
using _Project.Scripts.Services;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.Installers
{
    public class ShipInstaller : MonoInstaller<ShipInstaller>
    {
        [SerializeField] private AssetReference _weaponViewPrefabReference;
        [SerializeField] private AssetReference _projectilePrefabReference;

        private IScenesAssetProvider _assetProvider;

        [Inject]
        public void Construct(IScenesAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ShipStateProviderService>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerWeaponsInfoProviderService>()
                .AsSingle();

            InitializeProjectilePool();

            Container.BindInterfacesAndSelfTo<ProjectileFactory>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<WeaponFactory>()
                .AsSingle()
                .WithArguments(_weaponViewPrefabReference);

            Container.BindInterfacesAndSelfTo<PlayerShipFactory>()
                .AsSingle();
        }

        private void InitializeProjectilePool()
        {
            var projectilePrefab = _assetProvider.GetLoadedAsset<GameObject>(_projectilePrefabReference.AssetGUID);

            Container.BindMemoryPool<Projectile, Projectile.Pool>()
                .WithInitialSize(20)
                .FromComponentInNewPrefab(projectilePrefab)
                .UnderTransformGroup("Projectiles");
        }
    }
}
