using UnityEngine;

namespace Enemy.EnemyBehaviour
{
    public interface IEnemyBehaviour
    {
        public Vector3 CalculateVelocity(Vector3 currentPosition);
        public float CalculateTorque(Vector3 currentPosition, float currentRotation);
        
    }
}