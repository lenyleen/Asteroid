using System;
using DataObjects;
using Enemy.EnemyBehaviour;
using Interfaces;
using Projectiles;
using UniRx;
using UnityEngine;
using Zenject;

namespace Enemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Enemy : MonoBehaviour, ICollisionReceiver
    {
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private float _maxSpeed;
        [field: SerializeField] public ColliderType ColliderType { get; private set; }

        private IEnemyContext _context;
        private Action<Enemy> _onDeath;
        private CompositeDisposable _disposables = new ();
        private IEnemyBehaviour  _behaviour;
        private Vector2 _direction;

        private void Initialize(Vector3 position,IEnemyContext context, Action<Enemy> onDeath)
        {
            _context = context;
            transform.position = position;
            _onDeath = onDeath;
            
            _behaviour = context.Behaviour.Value;
            context.Behaviour.Subscribe(behaviour => _behaviour = behaviour)
                .AddTo(_disposables);

            context.OnDead.Subscribe(_ => _onDeath?.Invoke(this));
        }

        private void FixedUpdate()
        {
            var velocity = _behaviour.CalculateVelocity(transform.position);
            var torque = _behaviour.CalculateTorque(transform.position, _rb.rotation);
            _rb.AddTorque(torque);
            
            Vector2 forward = transform.right;
            if (_rb.linearVelocity.magnitude < _maxSpeed)
                _rb.AddRelativeForce(velocity);
        }

        public void Collide(ICollisionReceiver collisionReceiver)
        {
            _context.TakeCollision(collisionReceiver);
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.TryGetComponent(out ICollisionReceiver receiver))
                receiver.Collide(this);
        }

        private void Despawn()
        {
            _rb.linearVelocity = Vector2.zero;
            _rb.angularVelocity = 0;
            gameObject.SetActive(false);
            _disposables.Dispose();
            _disposables = new CompositeDisposable();
            _behaviour = null;
        }

        public class Pool : MonoMemoryPool<Vector3,IEnemyContext, Action<Enemy>,Enemy>
        {
            protected override void Reinitialize(Vector3 postion,IEnemyContext context,Action<Enemy> onDestroy,Enemy item)
            {
                item.Initialize(postion, context,onDestroy);
            }

            protected override void OnDespawned(Enemy item)
            {
                item.Despawn();
            }
        }
        
    }
}