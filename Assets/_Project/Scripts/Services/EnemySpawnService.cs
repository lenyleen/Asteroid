using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Configs;
using _Project.Scripts.Data;
using _Project.Scripts.Enemies;
using _Project.Scripts.Factories;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Static;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.Services
{
    public class EnemySpawnService : IAsyncInitializable, IDisposable, IEnemyDiedNotifier
    {
        private const string EnemyConfigsLabel = "EnemiesConfigs";

        public IObservable<KilledEnemyData> OnEnemyKilled => _enemyDiedCommand;

        private readonly Camera _camera;
        private readonly AssetProvider _assetProvider;
        private readonly CompositeDisposable _disposable = new();
        private readonly EnemyFactory _enemyFactory;
        private readonly SpawnConfig _spawnConfig;
        private readonly HashSet<ISpawnableEnemy> _spawnedEnemies;
        private readonly ReactiveCommand<KilledEnemyData> _enemyDiedCommand = new();
        private readonly IAnalyticsService _analyticsService;

        private bool _canSpawn;
        private CompositeDisposable _spawnDisposable = new();
        private Dictionary<EnemyType, EnemyConfig> _enemiesData;

        public EnemySpawnService(Camera camera, EnemyFactory enemyFactory, SpawnConfig spawnConfig,
            AssetProvider assetProvider, IAnalyticsService analyticsService)
        {
            _camera = camera;
            _enemyFactory = enemyFactory;
            _spawnConfig = spawnConfig;
            _spawnedEnemies = new HashSet<ISpawnableEnemy>();
            _assetProvider = assetProvider;
            _analyticsService = analyticsService;
        }

        public async UniTask InitializeAsync()
        {
            var enemyConfigs = await _assetProvider.LoadMany<EnemyConfig>(EnemyConfigsLabel);

            _enemiesData = enemyConfigs.ToDictionary(data => data.Type, data => data);

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

            _analyticsService.EnemyKilled(enemy.Type);

            if (enemy.Type != EnemyType.Asteroid)
                return;

            if (!_enemiesData.TryGetValue(EnemyType.LilAsteroid, out var data))
                return;

            for (var i = 0; i < _spawnConfig.SpawnLilAsteroidCount; i++)
            {
                var position = RandomPositionGenerator.GenerateRandomPositionNearPosition(enemy.Position.Value);
                SpawnEnemy(data, position);
            }
        }
    }
}
