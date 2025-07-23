using UnityEngine;

namespace Enemies.EnemyBehaviour
{
    public interface IEnemyBehaviour
    {
        public void Update(ref Vector3 currentPosition, Vector3 followingPosition,
            ref Vector2 currentVelocity, ref float currentRotation);
    }
}
