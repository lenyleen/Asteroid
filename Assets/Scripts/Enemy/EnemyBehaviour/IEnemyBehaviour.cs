using UnityEngine;

namespace Enemy.EnemyBehaviour
{
    public interface IEnemyBehaviour
    {
        public Vector3 Execute(Vector3 playerPosition);
    }
}