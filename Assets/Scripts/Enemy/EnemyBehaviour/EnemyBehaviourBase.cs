using DataObjects;
using Interfaces;
using UnityEngine;

namespace Enemy.EnemyBehaviour
{
    public abstract class EnemyBehaviourBase : IEnemyBehaviour
    {
        protected readonly IPositionProvider _positionProvider;
        protected readonly EnemyBehaviourData _data;
        protected Vector3 _direction;

        protected EnemyBehaviourBase(EnemyBehaviourData data,  IPositionProvider positionProvider)
        {
            _data = data;
            _positionProvider = positionProvider;
        }
        
        public abstract Vector3 CalculateVelocity(Vector3 curPosition);
        public abstract float CalculateTorque(Vector3 curPosition, float currentRotation);
    }
}