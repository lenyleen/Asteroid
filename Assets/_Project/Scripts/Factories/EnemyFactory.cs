using System;
using _Project.Scripts.Configs;
using _Project.Scripts.Enemies;
using _Project.Scripts.Enemies.EnemyBehaviour;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Factories
{
    public class EnemyFactory
    {
        private readonly IPlayerStateProviderService _dataProviderService;
        private readonly Enemy.Pool _enemyPool;
        private readonly IInstantiator _instantiator;
        private readonly AssetProvider _assetProvider;

        public EnemyFactory(Enemy.Pool enemyPool, IPlayerStateProviderService dataProviderService,
            IInstantiator instantiator, AssetProvider assetProvider)
        {
            _enemyPool = enemyPool;
            _dataProviderService = dataProviderService;
            _instantiator = instantiator;
            _assetProvider = assetProvider;
        }

        public async UniTask<EnemyViewModel> Create(Vector3 position, EnemyConfig config)
        {
            var behaviour = CreateBehaviour(config);
            var model = _instantiator.Instantiate<EnemyModel>(new object[]
            {
                config, behaviour, position,
                _dataProviderService.PositionProvider.Value
            });
            var viewModel =
                _instantiator.Instantiate<EnemyViewModel>(new object[]
                    { model, _dataProviderService.PositionProvider.Value });

            viewModel.Initialize();

            var enemySprite = await _assetProvider.Load<Sprite>(config.Sprite);

            var enemy = _enemyPool.Spawn(position, enemySprite, viewModel, view => _enemyPool.Despawn(view));

            return viewModel;
        }

        private IEnemyBehaviour CreateBehaviour(EnemyConfig config)
        {
            return config.Type switch
            {
                EnemyType.UFO => new ChasingBehaviour(config.BehaviourConfig),
                EnemyType.Asteroid => new FlyOutBehaviour(config.BehaviourConfig),
                EnemyType.LilAsteroid => new FlyOutBehaviour(config.BehaviourConfig),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
