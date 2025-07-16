using Projectiles;
using UniRx;
using UnityEngine;

namespace Interfaces
{
    public interface IProjectileBehaviour
    {
        public ReactiveCommand OnDeath { get; }
        public Vector2 CalculateVelocity(Vector3 position);
        public void Update(float deltaTime);

        public void Collided();
    }
}