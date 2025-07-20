using DataObjects; 
using UnityEngine;

namespace Enemy.EnemyBehaviour
{
    public class FlyOutBehaviour : EnemyBehaviourBase
    {
        public FlyOutBehaviour(EnemyBehaviourData data) 
            : base(data)
        {
        }

        public override void Update(ref Vector3 currentPosition,  Vector3 followingPosition, 
                ref Vector2 currentVelocity, ref float currentRotation)
        {
            if (_direction == Vector3.zero)
                _direction = -currentPosition.normalized  * _data.acceleration;

            currentVelocity = _direction;
            if(currentVelocity.magnitude > _data._maxSpeed)
                currentVelocity *= _data._maxSpeed;
            
            currentPosition += (Vector3)currentVelocity * Time.deltaTime;
        }
    }
}