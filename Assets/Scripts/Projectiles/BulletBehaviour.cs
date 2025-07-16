using Interfaces;
using UniRx;
using UnityEngine;

namespace Projectiles
{
    public class BulletBehaviour : ProjectileBehaviourBase
    {
        private Vector2 _currentDirection;
        private readonly float _speed;
        

        public BulletBehaviour(Vector2 direction, float speed, float lifetime) : base(direction, lifetime)
        {
            _speed = speed;
        }

        public override Vector2 CalculateVelocity(Vector3 position)
        {
            if(_currentDirection != Vector2.zero)
                return Vector2.zero;
            
            _currentDirection = Vector2.up * _speed;
            return _currentDirection;
        }

        public override void Collided()
        {
            OnDeath?.Execute();
        }
    }
}