using UnityEngine;

namespace Enemy.EnemyBehaviour
{
    public class ChasingBehaviour : IEnemyBehaviour
    {
        public Vector3 Execute(Vector3 playerPosition)
        {
            return playerPosition.normalized;
        }
    }
}