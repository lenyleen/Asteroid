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
            var model = new EnemyModel(data.Health,data,behaviour, position);
            var viewModel = new EnemyViewModel(model, _signalBus);
            viewModel.Initialize();
            
            var enemy = _enemyPool.Spawn(position,data.Sprite, viewModel,view => _enemyPool.Despawn(view));
            
            return viewModel;
        }
        
        private IEnemyBehaviour CreateBehaviour(EnemyData data)
        {
            switch (data.Type)
            {
                case EnemyType.UFO:
                    return new ChasingBehaviour(data.BehaviourData, _dataProvider.PositionProvider.Value);
                case EnemyType.Asteroid:
                    return new FlyOutBehaviour(data.BehaviourData, _dataProvider.PositionProvider.Value);
                case EnemyType.LilAsteroid:
                    return new FlyOutBehaviour(data.BehaviourData, _dataProvider.PositionProvider.Value);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}