using System;
using System.Collections.Generic;
using System.Linq;
using Configs;
using Enemies;
using Factories;
using Interfaces;
using Static;
using UniRx;
using UnityEngine;
using Zenject;

namespace Services
{
    public class EnemySpawnService : IInitializable, IDisposable, IEnemyDiedNotifier
    {
        public IObservable<KilledEnemyData> OnEnemyKilled => _enemyDiedCommand;

        private readonly Camera _camera;
        private readonly CompositeDisposable _disposable = new();
        private readonly Dictionary<EnemyType, EnemyConfig> _enemiesData;
        private readonly EnemyFactory _enemyFactory;
        private readonly SpawnConfig _spawnConfig;
        private readonly HashSet<ISpawnableEnemy> _spawnedEnemies;
        private readonly ReactiveCommand<KilledEnemyData> _enemyDiedCommand = new();

        private bool _canSpawn;
        private CompositeDisposable _spawnDisposable = new();

        public EnemySpawnService(Camera camera, EnemyFactory enemyFactory,
            List<EnemyConfig> enemiesData, SpawnConfig spawnConfig)
        {
            _camera = camera;
            _enemyFactory = enemyFactory;
            _enemiesData = enemiesData.ToDictionary(data => data.Type, data => data);
            _spawnConfig = spawnConfig;
            _spawnedEnemies = new HashSet<ISpawnableEnemy>();
        }

        public void Initialize()
        {
            foreach (var enemyData in _enemiesData.Values)
                Observable.Interval(TimeSpan.FromSeconds(enemyData.SpawnTimeInSeconds))
                    .Subscribe(_ => SpawnEnemy(enemyData))
                    .AddTo(_disposable);
        }

        public void EnableSpawn(bool enable)
        {
            _canSpawn = enable;
            if (_canSpawn)
                return;

            _spawnDisposable.Dispose();
            _spawnDisposable = new CompositeDisposable();

            foreach (var spawnableEnemy in _spawnedEnemies)
                spawnableEnemy.Despawn();

            _spawnedEnemies.Clear();
        }

        public void Dispose()
        {
            _disposable.Dispose();
            _spawnDisposable.Dispose();
        }

        private async void SpawnEnemy(EnemyConfig enemyConfig, Vector3 position = default)
        {
            if (!_canSpawn)
                return;

            if (_spawnConfig.MaxEnemies <= _spawnedEnemies.Count)
                return;

            position = position == default
                ? RandomPositionGenerator.GetRandomPositionOutsideCamera(_camera)
                : position;
            var enemy = await _enemyFactory.Create(position, enemyConfig);

            if (enemy == null)
                return;

            _spawnedEnemies.Add(enemy);

            enemy.OnDead.Take(1)
                .Subscribe(_ => OnEnemyDied(enemy))
                .AddTo(_spawnDisposable);
        }

        private void OnEnemyDied(ISpawnableEnemy enemy)
        {
            if (!_spawnedEnemies.Contains(enemy))
                return;

            _spawnedEnemies.Remove(enemy);
            _enemyDiedCommand.Execute(
                new KilledEnemyData(enemy.Type, enemy.Position.Value, enemy.Score));

            if (enemy.Type != EnemyType.Asteroid)
                return;

            if (!_enemiesData.TryGetValue(EnemyType.LilAsteroid, out var data))
                return;

            for (var i = 0; i < _spawnConfig.SpawnLilAsteroidCount; i++)
            {
                var postion = RandomPositionGenerator.GenerateRandomPositionNearPosition(enemy.Position.Value);
                SpawnEnemy(data, postion);
            }
        }
    }
}
