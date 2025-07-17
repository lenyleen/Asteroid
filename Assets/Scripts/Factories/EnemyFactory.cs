using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly SignalBus _signalBus;

        public EnemyFactory(Enemy.Enemy.Pool enemyPool,SignalBus signalBus, IPlayerPositionProvider dataProvider)
        {
            _enemyPool = enemyPool;
            _dataProvider = dataProvider;
            _signalBus = signalBus;
        }
        public EnemyViewModel Create(Vector3 position, EnemyData  data)
        {
            if (_dataProvider.PositionProvider.Value == null)
                return null;
            
            var behaviour = CreateBehaviour(data);
            var model = new EnemyModel(data.Health,data,behaviour, position, _dataProvider.PositionProvider.Value);
            var viewModel = new EnemyViewModel(model, _signalBus);
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