using _Project.Scripts.Configs;
using UnityEngine;

namespace _Project.Scripts.Enemies.EnemyBehaviour
{
    public class FlyOutBehaviour : EnemyBehaviourBase
    {
        public FlyOutBehaviour(EnemyBehaviourConfig config)
            : base(config)
        {
        }

        public override void Update(ref Vector3 currentPosition, Vector3 followingPosition,
            ref Vector2 currentVelocity, ref float currentRotation)
        {
            if (_direction == Vector3.zero)
                _direction = -currentPosition.normalized * Config.Acceleration;

            currentVelocity = _direction;

            if (currentVelocity.magnitude > Config.MaxSpeed)
                currentVelocity *= Config.MaxSpeed;

            currentPosition += (Vector3)currentVelocity * Time.deltaTime;
        }
    }
}
