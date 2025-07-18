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

        public abstract void Update(ref Vector3 currentPosition, Vector3 followingPosition,
            ref Vector2 currentVelocity, ref float currentRotation);
    }
}