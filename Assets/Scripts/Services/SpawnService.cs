using System;
using System.Collections.Generic;
using System.Timers;
using DataObjects;
using Enemy;
using Interfaces;
using Static;
using UniRx;
using UnityEngine;

using Zenject;

namespace Services
{
    public class SpawnService : IInitializable, IDisposable
    {
        private readonly Camera _camera;
        private readonly IFactory<Vector3, EnemyData, EnemyViewModel> _enemyFactory;
        private readonly List<EnemyData> _spawnData;
        private readonly CompositeDisposable _disposable = new ();
        private HashSet<IDieble> _spawnedEnemies;
        private readonly int _maxEnemies;
        
        private bool _canSpawn = true;

        public SpawnService(Camera camera, IFactory<Vector3, EnemyData, EnemyViewModel> enemyFactory, 
            List<EnemyData> spawnData, int maxEnemies)
        {
            _camera = camera;
            _enemyFactory = enemyFactory;
            _spawnData = spawnData;
            _maxEnemies = maxEnemies;
            _spawnedEnemies = new HashSet<IDieble>();
        }

        public void Initialize()
        {
            foreach (var enemyData in _spawnData)
            {
                Observable.Interval(TimeSpan.FromSeconds(enemyData.SpawnTimeInSeconds))
                    .Subscribe(_ => SpawnEnemy(enemyData))
                    .AddTo(_disposable);
            }
        }

        private void SpawnEnemy(EnemyData enemyData)
        {
            if(!_canSpawn)
                return;
            
            if(_maxEnemies <= _spawnedEnemies.Count )
                return;
            
            var position = RandomPositionGenerator.GetRandomPositionOutsideCamera(_camera);
            var enemy = _enemyFactory.Create(position, enemyData);
            
            if(enemy == null)
                return;
            
            enemy.OnDead.Take(1)
                .Subscribe(_ => OnEnemyDied(enemy));
        }

        private void OnEnemyDied(IDieble enemy)
        {
            if (!_spawnedEnemies.Contains(enemy))
                return;
            
            _spawnedEnemies.Remove(enemy);
            
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}