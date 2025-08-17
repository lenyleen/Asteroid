using _Project.Scripts.Configs;
using _Project.Scripts.Enemies;
using _Project.Scripts.Factories;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Services;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.Installers
{
    public class EnemyInstaller : MonoInstaller<EnemyInstaller>
    {
        [SerializeField] private AssetReference _enemyPrefabReference;

        private IScenesAssetProvider _assetProvider;

        [Inject]
        public void Construct(IScenesAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public override void InstallBindings()
        {
            InitializeEnemyPool();

            Container.Bind<EnemyFactory>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<EnemySpawnService>()
                .AsSingle();
        }

        void InitializeEnemyPool()
        {
            var enemyPrefab = _assetProvider.GetLoadedAsset<GameObject>(_enemyPrefabReference.AssetGUID);

            Container.BindMemoryPool<Enemy, Enemy.Pool>()
                .WithInitialSize(20)
                .FromComponentInNewPrefab(enemyPrefab)
                .UnderTransformGroup("Enemies");
        }
    }
}
