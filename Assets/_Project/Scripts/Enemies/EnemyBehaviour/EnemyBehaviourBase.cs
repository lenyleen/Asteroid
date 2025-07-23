using Configs;
using UnityEngine;

namespace Enemies.EnemyBehaviour
{
    public abstract class EnemyBehaviourBase : IEnemyBehaviour
    {
        protected readonly EnemyBehaviourConfig Config;

        protected Vector3 _direction;

        protected EnemyBehaviourBase(EnemyBehaviourConfig config)
        {
            Config = config;
        }

        public abstract void Update(ref Vector3 currentPosition, Vector3 followingPosition, ref Vector2 currentVelocity,
            ref float currentRotation);
    }
}
