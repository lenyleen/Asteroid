using UnityEngine;

namespace Enemy.EnemyBehaviour
{
    public class FlyOutBehaviour : IEnemyBehaviour
    {
        private Vector3 _direction;
        public Vector3 Execute(Vector3 playerPosition)
        {
            if(_direction != Vector3.zero)
                return Vector3.zero;

            _direction = playerPosition.normalized;
            return _direction;
        }
    }
}