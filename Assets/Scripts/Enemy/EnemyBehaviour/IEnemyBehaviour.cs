using UnityEngine;

namespace Enemy.EnemyBehaviour
{
    public interface IEnemyBehaviour
    {
        public Vector3 CalculateVelocity(Vector3 currentPosition,  Vector3 followingPosition);
        public float CalculateTorque(Vector3 currentPosition, float currentRotation);
        
    }
}