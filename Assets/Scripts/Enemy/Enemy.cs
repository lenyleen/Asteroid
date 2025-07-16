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
    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(BoxCollider2D))]
    public class Enemy : MonoBehaviour, ICollisionReceiver
    {
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private SpriteRenderer _renderer; 
        [SerializeField] private float _maxSpeed;
        [SerializeField] private BoxCollider2D _collider;
        [field: SerializeField] public ColliderType ColliderType { get; private set; }

        private IEnemyContext _context;
        private Action<Enemy> _onDeath;
        private CompositeDisposable _disposables = new ();
        private IEnemyBehaviour  _behaviour;
        private Vector2 _direction;

        private void Initialize(Vector3 position,Sprite sprite,IEnemyContext context, Action<Enemy> onDeath)
        {
            _context = context;
            transform.position = position;
            _onDeath = onDeath;
            _renderer.sprite = sprite;
            
            _collider.size = sprite.bounds.size;
            
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
            
            _context.UpdatePosition(transform.position);
        }

        public void Collide(ICollisionReceiver collisionReceiver)
        {
            _context.TakeCollision(collisionReceiver);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(!other.gameObject.TryGetComponent(out ICollisionReceiver receiver))
                return;
            
            if(receiver.ColliderType != ColliderType.Player) return;
                
            receiver.Collide(this);
            Debug.Log("Collided");
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

        public class Pool : MonoMemoryPool<Vector3,Sprite,IEnemyContext, Action<Enemy>,Enemy>
        {
            protected override void Reinitialize(Vector3 postion,Sprite sprite,IEnemyContext context,Action<Enemy> onDestroy,Enemy item)
            {
                item.Initialize(postion,sprite, context,onDestroy);
            }

            protected override void OnDespawned(Enemy item)
            {
                item.Despawn();
            }
        }
        
    }
}