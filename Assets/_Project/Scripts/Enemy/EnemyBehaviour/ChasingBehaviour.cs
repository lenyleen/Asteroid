using DataObjects;
using Interfaces;
using UnityEngine;

namespace Enemy.EnemyBehaviour
{
    public class ChasingBehaviour : EnemyBehaviourBase
    {
        public ChasingBehaviour(EnemyBehaviourData data) 
            : base(data)
        {
        }

        public override void Update(ref Vector3 currentPosition,  Vector3 followingPosition, 
            ref Vector2 currentVelocity, ref float currentRotation)
        {
            _direction = (followingPosition - currentPosition).normalized;
            _direction *= _data.acceleration;
            currentRotation = CalculateRotation(currentRotation);

            currentVelocity = _direction * Time.deltaTime;
           
            if(currentVelocity.magnitude > _data._maxSpeed)
               currentVelocity *= _data._maxSpeed;
           
            currentPosition += (Vector3)currentVelocity;
        }

        private float CalculateRotation(float currentRotation)
        {
            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            float angleDiff = Mathf.DeltaAngle(currentRotation, angle);
            float torque = Mathf.Clamp(angleDiff, -1f, 1f) * _data.angularAcceleration;
            
            return torque;
        }
    }
}