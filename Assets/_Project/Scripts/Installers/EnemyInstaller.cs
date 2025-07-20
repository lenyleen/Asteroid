using System.Collections.Generic;
using DataObjects;
using Factories;
using Services;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class EnemyInstaller : MonoInstaller<EnemyInstaller>
    {
        [SerializeField]private Enemy.Enemy _enemyPrefab;
        [SerializeField]private List<EnemyData> _enemyDatas;
        [SerializeField]private SpawnData _spawnData;

        
        public override void InstallBindings()
        {
            Container.BindMemoryPool<Enemy.Enemy, Enemy.Enemy.Pool>().WithInitialSize(20)
                .FromComponentInNewPrefab(_enemyPrefab).UnderTransformGroup("Enemies");

            Container.BindInterfacesAndSelfTo<EnemyFactory>().AsSingle();

            Container.BindInterfacesAndSelfTo<SpawnService>().AsSingle().WithArguments(_enemyDatas, _spawnData);
        }
    }
}