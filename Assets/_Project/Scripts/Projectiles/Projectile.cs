using System;
using _Project.Scripts.Configs;
using _Project.Scripts.Interfaces;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Projectiles
{
    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(BoxCollider2D))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private BoxCollider2D _collider;

        private IProjectileBehaviour _behaviour;
        private ProjectileConfig _projectileConfig;
        private float _lifetime;
        private Action<Projectile> _onDeath;

        private void Initialize(ProjectileInitData data, Action<Projectile> onDeath)
        {
            _projectileConfig = data.Config;
            _lifetime = _projectileConfig.LifetimeInSeconds;
            _behaviour = data.Behaviour;
            _onDeath = onDeath;

            _renderer.sprite = _projectileConfig.Sprite;
            _collider.size = _projectileConfig.Sprite.bounds.size;
            _collider.offset = _projectileConfig.Sprite.bounds.center;
            gameObject.SetActive(true);

            _rb.position = data.Position;
            _rb.rotation = data.Rotation;
            _rb.linearVelocity = data.Velocity;

            _behaviour.Initialize(data.Position, data.Rotation);
        }

        private void Update()
        {
            UpdateMovement();

            UpdateLifetime();
        }

        private void UpdateMovement()
        {
            var pos = (Vector3)_rb.position;
            var rot = _rb.rotation;
            var vel = _rb.linearVelocity;

            _behaviour.Update(ref pos, ref rot, ref vel);

            _rb.position = pos;
            _rb.rotation = rot;
            _rb.linearVelocity = vel;
        }

        private void UpdateLifetime()
        {
            _lifetime -= Time.deltaTime;

            if (_lifetime <= 0)
                Die();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent<ICollisionReceiver>(out var receiver))
                return;

            if (receiver.ColliderType != ColliderType.Enemy)
                return;

            receiver.Collide(_projectileConfig.ColliderConfig.ColliderType,
                _projectileConfig.ColliderConfig.Damage);

            if (_behaviour.CheckDeathAfterCollision())
                Die();
        }

        private void Die()
        {
            _behaviour.Dispose();
            _onDeath?.Invoke(this);
        }

        private void Despawn()
        {
            gameObject.SetActive(false);
            _renderer.sprite = null;
            _rb.linearVelocity = Vector2.zero;
            _behaviour = null;
        }

        public class Pool : MonoMemoryPool<ProjectileInitData, Action<Projectile>, Projectile>
        {
            protected override void Reinitialize(ProjectileInitData data, Action<Projectile> onDeath, Projectile item)
            {
                item.Initialize(data, onDeath);
            }

            protected override void OnDespawned(Projectile item)
            {
                item.Despawn();
            }
        }
    }

    public class ProjectileInitData
    {
        public ProjectileConfig Config {get;}
        public IProjectileBehaviour Behaviour {get;}
        public Vector3 Position {get;}
        public float Rotation {get;}
        public Vector2 Velocity {get;}

        public ProjectileInitData(ProjectileConfig config, IProjectileBehaviour behaviour, Vector3 position, float rotation, Vector2 velocity)
        {
            Config = config;
            Behaviour = behaviour;
            Position = position;
            Rotation = rotation;
            Velocity = velocity;
        }
    }
}

