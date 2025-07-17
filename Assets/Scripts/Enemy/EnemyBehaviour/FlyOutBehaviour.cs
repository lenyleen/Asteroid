using DataObjects;
using Interfaces;
using UnityEngine;

namespace Enemy.EnemyBehaviour
{
    public class FlyOutBehaviour : EnemyBehaviourBase
    {
        public FlyOutBehaviour(EnemyBehaviourData data) 
            : base(data)
        {
        }

        public override Vector3 CalculateVelocity(Vector3 currentPosition, Vector3 followingPosition)
        {
            if (_direction != Vector3.zero)
                return _direction;
            
            _direction = -currentPosition.normalized  * _data.acceleration;
            return _direction;
        }

        public override float CalculateTorque(Vector3 curPosition, float currentRotation)
        {
            return 0;
        }
    }
}