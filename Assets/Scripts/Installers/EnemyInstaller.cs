using System.Collections.Generic;
using DataObjects;
using Factories;
using Services;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class EnemyInstaller : Installer<Enemy.Enemy,SpawnData,List<EnemyData>,EnemyInstaller>
    {
        private Enemy.Enemy _enemyPrefab;
        private List<EnemyData> _enemyDatas;
        private SpawnData _spawnData;

        public EnemyInstaller(Enemy.Enemy enemyPrefab,SpawnData spawnData, List<EnemyData> enemyDatas)
        {
            _enemyPrefab = enemyPrefab;
            _enemyDatas = enemyDatas;
            _spawnData = spawnData;
        }
        public override void InstallBindings()
        {
            Container.BindMemoryPool<Enemy.Enemy, Enemy.Enemy.Pool>().WithInitialSize(20)
                .FromComponentInNewPrefab(_enemyPrefab).UnderTransformGroup("Enemies");

            Container.BindInterfacesAndSelfTo<EnemyFactory>().AsSingle();

            Container.BindInterfacesAndSelfTo<SpawnService>().AsSingle().WithArguments(_enemyDatas, _spawnData);
        }
    }
}