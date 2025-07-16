using System.Collections.Generic;
using DataObjects;
using Factories;
using Services;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class EnemyInstaller : Installer<Enemy.Enemy,int,List<EnemyData>,EnemyInstaller>
    {
        private Enemy.Enemy _enemyPrefab;
        private List<EnemyData> _enemyDatas;
        private int _maxEnemies;

        public EnemyInstaller(Enemy.Enemy enemyPrefab,int maxEnemies, List<EnemyData> enemyDatas)
        {
            _enemyPrefab = enemyPrefab;
            _enemyDatas = enemyDatas;
            _maxEnemies = maxEnemies;
        }
        public override void InstallBindings()
        {
            Container.BindMemoryPool<Enemy.Enemy, Enemy.Enemy.Pool>().WithInitialSize(20)
                .FromComponentInNewPrefab(_enemyPrefab).UnderTransformGroup("Enemies");

            Container.BindInterfacesAndSelfTo<EnemyFactory>().AsSingle();

            Container.BindInterfacesAndSelfTo<SpawnService>().AsSingle().WithArguments(Camera.main, _enemyDatas, _maxEnemies);
        }
    }
}