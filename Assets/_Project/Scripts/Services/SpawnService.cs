using System;
using System.Collections.Generic;
using System.Linq;
using Configs;
using Enemies;
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
        private readonly CompositeDisposable _disposable = new();
        private readonly Dictionary<EnemyType, EnemyConfig> _enemiesData;
        private readonly IFactory<Vector3, EnemyConfig, EnemyViewModel> _enemyFactory;
        private readonly IGameEvents _gameEvents;
        private readonly SpawnConfig _spawnConfig;
        private readonly HashSet<ISpawnableEnemy> _spawnedEnemies;
        private bool _canSpawn;

        private CompositeDisposable _spawnDisposable = new();

        public SpawnService(Camera camera, IFactory<Vector3, EnemyConfig, EnemyViewModel> enemyFactory,
            List<EnemyConfig> enemiesData, SpawnConfig spawnConfig, IGameEvents gameEvents)
        {
            _camera = camera;
            _enemyFactory = enemyFactory;
            _enemiesData = enemiesData.ToDictionary(data => data.Type, data => data);
            _spawnConfig = spawnConfig;
            _gameEvents = gameEvents;
            _spawnedEnemies = new HashSet<ISpawnableEnemy>();
        }

        public void Dispose()
        {
            _disposable.Dispose();
            _spawnDisposable.Dispose();
        }

        public void Initialize()
        {
            foreach (var enemyData in _enemiesData.Values)
            {
                Observable.Interval(TimeSpan.FromSeconds(enemyData.SpawnTimeInSeconds))
                    .Subscribe(_ => SpawnEnemy(enemyData))
                    .AddTo(_disposable);
            }

            _gameEvents.OnGameStarted.Subscribe(_ =>
                    EnableSpawn(true))
                .AddTo(_disposable);

            _gameEvents.OnGameEnded.Subscribe(_ =>
                    EnableSpawn(false))
                .AddTo(_disposable);
        }

        private void SpawnEnemy(EnemyConfig enemyConfig, Vector3 position = default)
        {
            if (!_canSpawn)
            {
                return;
            }

            if (_spawnConfig.MaxEnemies <= _spawnedEnemies.Count)
            {
                return;
            }

            position = position == default
                ? RandomPositionGenerator.GetRandomPositionOutsideCamera(_camera)
                : position;

            var enemy = _enemyFactory.Create(position, enemyConfig);

            if (enemy == null)
            {
                return;
            }

            _spawnedEnemies.Add(enemy);
            enemy.OnDead.Take(1)
                .Subscribe(_ => OnEnemyDied(enemy))
                .AddTo(_spawnDisposable);
        }

        private void EnableSpawn(bool enable)
        {
            _canSpawn = enable;
            if (_canSpawn)
            {
                return;
            }

            _spawnDisposable.Dispose();
            _spawnDisposable = new CompositeDisposable();

            foreach (var spawnableEnemy in _spawnedEnemies)
            {
                spawnableEnemy.Despawn();
            }

            _spawnedEnemies.Clear();
        }

        private void OnEnemyDied(ISpawnableEnemy enemy)
        {
            if (!_spawnedEnemies.Contains(enemy))
            {
                return;
            }

            _spawnedEnemies.Remove(enemy);
            _gameEvents.PlayerReceivedScore(enemy.Score);

            if (enemy.Type != EnemyType.Asteroid)
            {
                return;
            }

            if (!_enemiesData.TryGetValue(EnemyType.LilAsteroid, out var data))
            {
                return;
            }

            for (var i = 0; i < _spawnConfig.SpawnLilAsteroidCount; i++)
            {
                var postion = RandomPositionGenerator.GenerateRandomPositionNearPosition(enemy.Position.Value);
                SpawnEnemy(data, postion);
            }
        }
    }
}
