using DataObjects;
using Interfaces;
using UnityEngine;

namespace Enemy.EnemyBehaviour
{
    public class ChasingBehaviour : EnemyBehaviourBase
    {
        public ChasingBehaviour(EnemyBehaviourData data, IPositionProvider positionProvider) 
            : base(data, positionProvider)
        {
        }

        public override Vector3 CalculateVelocity(Vector3 currentPosition)
        {
            _direction = (_positionProvider.Position - currentPosition).normalized;
            _direction *= _data.acceleration;
            return _direction;
        }

        public override float CalculateTorque(Vector3 currentPosition, float currentRotation)
        {
            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            float angleDiff = Mathf.DeltaAngle(currentRotation, angle);
            float torque = Mathf.Clamp(angleDiff, -1f, 1f) * _data.angularAcceleration;
            
            return torque;
        }
    }
}