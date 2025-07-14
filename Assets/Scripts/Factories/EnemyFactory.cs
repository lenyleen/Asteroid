using System.Collections.Generic;
using System.Linq;
using DataObjects;
using Enemy;
using Interfaces;
using Zenject;

namespace Factories
{
    public class EnemyFactory : IFactory<EnemyData, EnemyViewModel>
    {
        private readonly DiContainer _diContainer;
        private readonly Enemy.Enemy.Pool _enemyPool;
        private readonly IPositionProvider _positionProvider;
        private readonly Dictionary<EnemyType, EnemyData> _enemyData;

        public EnemyFactory(DiContainer diContainer, Enemy.Enemy.Pool enemyPool, IPositionProvider positionProvider,
            List<EnemyData> enemyData)
        {
            _diContainer = diContainer;
            _enemyPool = enemyPool;
            _positionProvider = positionProvider;
            _enemyData = enemyData.ToDictionary(e => e.Type);
        }
        public EnemyViewModel Create(EnemyData param)
        {
            
        }
    }
}