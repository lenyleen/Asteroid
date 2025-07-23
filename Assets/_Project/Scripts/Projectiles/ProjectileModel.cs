using Configs;
using Interfaces;
using UniRx;
using UnityEngine;

namespace Projectiles
{
    public class ProjectileModel
    {
        private readonly IProjectileBehaviour _behaviour;

        private readonly ColliderConfig _colliderConfig;
        public readonly ReactiveCommand OnDeath = new();
        public readonly ReactiveProperty<Vector3> Position;
        public readonly ReactiveProperty<float> Rotation;
        public readonly ReactiveProperty<Vector2> Velocity;

        private float _lifetime;

        public ProjectileModel(ProjectileConfig config, IProjectileBehaviour behaviour,
            Vector3 position, float rotation, Vector2 velocity)
        {
            Position = new ReactiveProperty<Vector3>(position);
            Rotation = new ReactiveProperty<float>(rotation);
            Velocity = new ReactiveProperty<Vector2>(velocity);

            _colliderConfig = config.ColliderConfig;
            _behaviour = behaviour;
            _lifetime = config.LifetimeInSeconds;

            _behaviour.Initialize(position, rotation);
        }

        public ColliderType ColliderType => _colliderConfig.ColliderType;
        public int Damage => _colliderConfig.Damage;

        public void UpdateMovement()
        {
            var curPosition = Position.Value;
            var curRotation = Rotation.Value;
            var curVelocity = Velocity.Value;

            _behaviour.Update(ref curPosition, ref curRotation, ref curVelocity);

            Rotation.Value = curRotation;
            Velocity.Value = curVelocity;
            Position.Value = curPosition;
        }

        public void TakeHit()
        {
            if (!_behaviour.CheckDeathAfterCollision())
            {
                return;
            }

            Die();
        }

        public void UpdateLifetime()
        {
            _lifetime -= Time.deltaTime;

            if (_lifetime < 0)
            {
                Die();
            }
        }

        private void Die()
        {
            OnDeath.Execute();
            _behaviour.Dispose();
        }
    }
}
