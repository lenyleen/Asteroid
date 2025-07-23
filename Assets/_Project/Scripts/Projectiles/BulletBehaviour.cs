using Interfaces;
using UnityEngine;

namespace Projectiles
{
    public class BulletBehaviour : IProjectileBehaviour
    {
        private readonly float _speed;

        private Vector2 _direction;

        public BulletBehaviour(float speed)
        {
            _speed = speed;
        }

        public void Initialize(Vector3 spawnPosition, float shooterRotation)
        {
            var forward = Quaternion.Euler(0, 0, -shooterRotation) * Vector3.up;
            _direction = new Vector2(-forward.x, forward.y).normalized;
        }

        public void Update(ref Vector3 position, ref float rotation, ref Vector2 velocity)
        {
            velocity = _direction * _speed;
            position += (Vector3)(velocity * Time.deltaTime);
        }

        public bool CheckDeathAfterCollision()
        {
            return true;
        }

        public void Dispose() { }
    }
}
