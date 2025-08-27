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
        private float _lifetime;
        private Action<Projectile> _onDeath;
        private ProjectileInitData _data;

        private void Initialize(ProjectileInitData data, Action<Projectile> onDeath)
        {
            _data = data;

            _lifetime = _data.LifetimeInSeconds;
            _behaviour = _data.Behaviour;
            _onDeath = onDeath;

            _renderer.sprite = _data.Sprite;
            _collider.size = _data.Sprite.bounds.size;
            _collider.offset = _data.Sprite.bounds.center;

            _renderer.enabled = data.EnableSprite;

            gameObject.SetActive(true);

            _rb.position = _data.Position;
            _rb.rotation = _data.Rotation;
            _rb.linearVelocity = _data.Velocity;

            _behaviour.Initialize(_data.Position, _data.Rotation);
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

            receiver.Collide(_data.ColliderConfig.ColliderType,
                _data.ColliderConfig.Damage);

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
        public Sprite Sprite { get; }
        public ColliderConfig ColliderConfig { get; }
        public float LifetimeInSeconds { get; }
        public IProjectileBehaviour Behaviour {get;}
        public Vector3 Position {get;}
        public float Rotation {get;}
        public Vector2 Velocity {get;}

        public bool EnableSprite {get;}

        public ProjectileInitData(Sprite sprite, IProjectileBehaviour behaviour, Vector3 position, float rotation,
            Vector2 velocity, ColliderConfig colliderConfig, float lifetimeInSeconds, bool enableSprite)
        {
            Sprite = sprite;
            EnableSprite = enableSprite;
            Behaviour = behaviour;
            Position = position;
            Rotation = rotation;
            Velocity = velocity;
            ColliderConfig = colliderConfig;
            LifetimeInSeconds = lifetimeInSeconds;
        }
    }
}

