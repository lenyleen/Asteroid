using DataObjects;
using Interfaces;
using UnityEngine;

namespace Enemy.EnemyBehaviour
{
    public abstract class EnemyBehaviourBase : IEnemyBehaviour
    {
        protected readonly EnemyBehaviourData _data;
        protected Vector3 _direction;

        protected EnemyBehaviourBase(EnemyBehaviourData data)
        {
            _data = data;
        }
        
        public abstract Vector3 CalculateVelocity(Vector3 curPosition, Vector3 followingPosition);
        public abstract float CalculateTorque(Vector3 curPosition, float currentRotation);
    }
}