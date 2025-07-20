using System;
using DataObjects;
using Enemy;
using Enemy.EnemyBehaviour;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Factories
{
    public class EnemyFactory : IFactory<Vector3, EnemyData, EnemyViewModel>
    {
        private readonly Enemy.Enemy.Pool _enemyPool;
        private readonly IPlayerPositionProvider _dataProvider;
        private readonly IInstantiator _instantiator;

        public EnemyFactory(Enemy.Enemy.Pool enemyPool, IPlayerPositionProvider dataProvider,
            IInstantiator instantiator)
        {
            _enemyPool = enemyPool;
            _dataProvider = dataProvider;
            _instantiator = instantiator;
        }
        public EnemyViewModel Create(Vector3 position, EnemyData  data)
        {
            var behaviour = CreateBehaviour(data);
            var model = _instantiator.Instantiate<EnemyModel>(new object[]{data, behaviour, position,
                _dataProvider.PositionProvider.Value});
            var viewModel = _instantiator.Instantiate<EnemyViewModel>(new object[]{model, _dataProvider.PositionProvider.Value});
            
            viewModel.Initialize();
            
            var enemy = _enemyPool.Spawn(position,data.Sprite, viewModel,view => _enemyPool.Despawn(view));
            
            return viewModel;
        }
        
        private IEnemyBehaviour CreateBehaviour(EnemyData data)
        {
            return data.Type switch
            {
                EnemyType.UFO => new ChasingBehaviour(data.BehaviourData),
                EnemyType.Asteroid => new FlyOutBehaviour(data.BehaviourData),
                EnemyType.LilAsteroid => new FlyOutBehaviour(data.BehaviourData),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}