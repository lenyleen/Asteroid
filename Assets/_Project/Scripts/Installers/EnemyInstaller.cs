using System.Collections.Generic;
using Configs;
using Enemies;
using Factories;
using Services;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class EnemyInstaller : MonoInstaller<EnemyInstaller>
    {
        [SerializeField] private Enemy _enemyPrefab;
        [SerializeField] private List<EnemyConfig> _enemyDatas;
        [SerializeField] private SpawnConfig spawnConfig;

        public override void InstallBindings()
        {
            Container.BindMemoryPool<Enemy, Enemy.Pool>().WithInitialSize(20)
                .FromComponentInNewPrefab(_enemyPrefab).UnderTransformGroup("Enemies");

            Container.BindInterfacesAndSelfTo<EnemyFactory>().AsSingle();

            Container.BindInterfacesAndSelfTo<EnemySpawnService>().AsSingle().WithArguments(_enemyDatas, spawnConfig);
        }
    }
}
