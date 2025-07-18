using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly Dictionary<EnemyType,EnemyData> _enemiesData;
        private readonly CompositeDisposable _disposable = new ();
        private readonly HashSet<ISpawnableEnemy> _spawnedEnemies;
        private readonly SpawnData _spawnData;

        private bool _canSpawn;

        public SpawnService(Camera camera, IFactory<Vector3, EnemyData, EnemyViewModel> enemyFactory, 
            List<EnemyData> enemiesData, SpawnData spawnData)
        {
            _camera = camera;
            _enemyFactory = enemyFactory;
            _enemiesData = enemiesData.ToDictionary(data => data.Type, data => data);
            _spawnData = spawnData;
            _spawnedEnemies = new HashSet<ISpawnableEnemy>();
        }

        public void Initialize()
        {
            foreach (var enemyData in _enemiesData.Values)
            {
                Observable.Interval(TimeSpan.FromSeconds(enemyData.SpawnTimeInSeconds))
                    .Subscribe(_ => SpawnEnemy(enemyData))
                    .AddTo(_disposable);
            }
        }
        
        public void EnableSpawn(bool enable) => _canSpawn = enable; //TODO
        
        private void SpawnEnemy(EnemyData enemyData, Vector3 position = default)
        {
            if(!_canSpawn)
                return;
            
            if(_spawnData.MaxEnemies <= _spawnedEnemies.Count )
                return;

            position = position == default
                ? RandomPositionGenerator.GetRandomPositionOutsideCamera(_camera)
                : position;
            
            var enemy = _enemyFactory.Create(position, enemyData);
            
            if(enemy == null)
                return;

            _spawnedEnemies.Add(enemy);
            enemy.OnDead.Take(1)
                .Subscribe(_ => OnEnemyDied(enemy));
        }
        
        private void OnEnemyDied(ISpawnableEnemy enemy)
        {
            if (!_spawnedEnemies.Contains(enemy))
                return;
            
            _spawnedEnemies.Remove(enemy);
            
            if(enemy.Type != EnemyType.Asteroid)
                return;

            if(!_enemiesData.TryGetValue(EnemyType.LilAsteroid, out var data))
                return;
            
            for (int i = 0; i < _spawnData.SpawnLilAsteroidCount; i++)
            {
                var postion = RandomPositionGenerator.GenerateRandomPositionNearPosition(enemy.Position.Value);
                SpawnEnemy(data, postion);
            }
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}