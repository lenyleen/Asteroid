using System;
using Configs;
using Enemies;
using Enemies.EnemyBehaviour;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Factories
{
    public class EnemyFactory : IFactory<Vector3, EnemyConfig, EnemyViewModel>
    {
        private readonly IPlayerPositionProvider _dataProvider;
        private readonly Enemy.Pool _enemyPool;
        private readonly IInstantiator _instantiator;

        public EnemyFactory(Enemy.Pool enemyPool, IPlayerPositionProvider dataProvider,
            IInstantiator instantiator)
        {
            _enemyPool = enemyPool;
            _dataProvider = dataProvider;
            _instantiator = instantiator;
        }

        public EnemyViewModel Create(Vector3 position, EnemyConfig config)
        {
            var behaviour = CreateBehaviour(config);
            var model = _instantiator.Instantiate<EnemyModel>(new object[]
            {
                config, behaviour, position,
                _dataProvider.PositionProvider.Value
            });
            var viewModel =
                _instantiator.Instantiate<EnemyViewModel>(new object[] { model, _dataProvider.PositionProvider.Value });

            viewModel.Initialize();

            var enemy = _enemyPool.Spawn(position, config.Sprite, viewModel, view => _enemyPool.Despawn(view));

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
