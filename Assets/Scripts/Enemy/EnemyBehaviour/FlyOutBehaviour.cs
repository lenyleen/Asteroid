using DataObjects;
using Interfaces;
using UnityEngine;

namespace Enemy.EnemyBehaviour
{
    public class FlyOutBehaviour : EnemyBehaviourBase
    {
        public FlyOutBehaviour(EnemyBehaviourData data, IPositionProvider positionProvider) 
            : base(data, positionProvider)
        {
        }

        public override Vector3 CalculateVelocity(Vector3 playerPosition)
        {
            if(_direction != Vector3.zero)
                return Vector3.zero;

            _direction = -playerPosition.normalized  * _data.acceleration;
            return _direction;
        }

        public override float CalculateTorque(Vector3 curPosition, float currentRotation)
        {
            return 0;
        }
    }
}