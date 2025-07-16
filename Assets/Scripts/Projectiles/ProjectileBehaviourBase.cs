using Interfaces;
using UniRx;
using UnityEngine;

namespace Projectiles
{
    public abstract class ProjectileBehaviourBase : IProjectileBehaviour
    {
        public ReactiveCommand OnDeath { get; }
        
        protected Vector2 _moveDirection;
        private float _lifetime;

        protected ProjectileBehaviourBase(Vector2 moveDirection, float lifetime)
        {
            _moveDirection = moveDirection;
            _lifetime = lifetime;
            OnDeath = new ReactiveCommand();
        }
        
        public abstract Vector2 CalculateVelocity(Vector3 position);
        public abstract void Collided();

        public void Update(float deltaTime)
        {
            _lifetime -= deltaTime;
            if(_lifetime <= 0 )
                OnDeath.Execute();
        }
    }
}