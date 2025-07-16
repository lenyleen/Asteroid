using Interfaces;
using UniRx;
using UnityEngine;

namespace Projectiles
{
    public class LaserBehaviour : ProjectileBehaviourBase
    {
        public LaserBehaviour(Vector2 moveDirection, float lifetime) : base(moveDirection, lifetime)
        {
        }

        public override Vector2 CalculateVelocity(Vector3 position)
        {
            return Vector2.zero;
        }
        public override void Collided()
        {
        }
    }
}