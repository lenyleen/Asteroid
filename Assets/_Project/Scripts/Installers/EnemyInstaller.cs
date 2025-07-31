using System.Collections.Generic;
using _Project.Scripts.Configs;
using _Project.Scripts.Enemies;
using _Project.Scripts.Factories;
using _Project.Scripts.Services;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Installers
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
