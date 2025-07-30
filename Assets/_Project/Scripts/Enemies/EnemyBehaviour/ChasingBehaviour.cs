using Configs;
using UnityEngine;

namespace Enemies.EnemyBehaviour
{
    public class ChasingBehaviour : EnemyBehaviourBase
    {
        public ChasingBehaviour(EnemyBehaviourConfig config)
            : base(config)
        {
        }

        public override void Update(ref Vector3 currentPosition, Vector3 followingPosition,
            ref Vector2 currentVelocity, ref float currentRotation)
        {
            _direction = (followingPosition - currentPosition).normalized;
            _direction *= Config.Acceleration;
            currentRotation = CalculateRotation(currentRotation);

            currentVelocity = _direction * Time.deltaTime;

            if (currentVelocity.magnitude > Config.MaxSpeed)
                currentVelocity *= Config.MaxSpeed;

            currentPosition += (Vector3)currentVelocity;
        }

        private float CalculateRotation(float currentRotation)
        {
            var angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            var angleDiff = Mathf.DeltaAngle(currentRotation, angle);
            var torque = Mathf.Clamp(angleDiff, -1f, 1f) * Config.AngularAcceleration;

            return torque;
        }
    }
}
