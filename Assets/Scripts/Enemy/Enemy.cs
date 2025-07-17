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

        private EnemyViewModel _context;
        private Action<Enemy> _onDeath;
        private CompositeDisposable _disposables = new ();
        private Vector2 _direction;

        private void Initialize(Vector3 position,Sprite sprite,EnemyViewModel context, Action<Enemy> onDeath)
        {
            _context = context;
            transform.position = position;
            _onDeath = onDeath;
            _renderer.sprite = sprite;
            _collider.enabled = true;
            
            _collider.size = sprite.bounds.size;
            
            _context.Position.Subscribe(pos => _rb.position = pos)
                .AddTo(_disposables);
                
            _context.Rotation.Subscribe(rot => _rb.rotation = rot)
                .AddTo(_disposables);
                
            _context.Velocity.Subscribe(vel => _rb.linearVelocity = vel)
                .AddTo(_disposables);

            _context.OnDead.Subscribe(_ => _onDeath?.Invoke(this))
                .AddTo(_disposables);
        }

        private void FixedUpdate()
        {
            _context.UpdatePosition();
        }

        public void Collide(ICollisionReceiver collisionReceiver)
        {
            if(gameObject.activeInHierarchy)
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
            _collider.enabled = false;
            _disposables.Dispose();
            _disposables = new CompositeDisposable();
        }

        public class Pool : MonoMemoryPool<Vector3,Sprite,EnemyViewModel, Action<Enemy>,Enemy>
        {
            protected override void Reinitialize(Vector3 postion,Sprite sprite,EnemyViewModel context,Action<Enemy> onDestroy,Enemy item)
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