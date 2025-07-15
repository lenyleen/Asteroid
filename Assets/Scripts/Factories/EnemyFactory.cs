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
        private readonly IPositionProvider _positionProvider;

        public EnemyFactory(Enemy.Enemy.Pool enemyPool, IPositionProvider positionProvider)
        {
            _enemyPool = enemyPool;
            _positionProvider = positionProvider;
        }
        public EnemyViewModel Create(Vector3 position, EnemyData  data)
        {
            var behaviour = CreateBehaviour(data);
            var model = new EnemyModel(data.Health);
            var viewModel = new EnemyViewModel(model);
            viewModel.Initialize();
            
            var enemy = _enemyPool.Spawn(position, viewModel,view => _enemyPool.Despawn(view));
            
            viewModel.SetBehaviour(behaviour);
            
            return viewModel;
        }
        
        private IEnemyBehaviour CreateBehaviour(EnemyData data)
        {
            switch (data.Type)
            {
                case EnemyType.UFO:
                    return new ChasingBehaviour(data.BehaviourData, _positionProvider);
                case EnemyType.Asteroid:
                    return new FlyOutBehaviour(data.BehaviourData, _positionProvider);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}