using System;
using _Project.Scripts.Configs;
using _Project.Scripts.Data;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Services;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Enemies
{
    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(BoxCollider2D))]
    public class Enemy : MonoBehaviour, ICollisionReceiver
    {
        [field: SerializeField] public ColliderType ColliderType { get; private set; }

        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private float _maxSpeed;
        [SerializeField] private BoxCollider2D _collider;

        private Vector2 _direction;
        private CompositeDisposable _disposables = new();
        private Action<Enemy> _onDeath;
        private EnemyViewModel _viewModel;
        private VfxService  _vfxService;
        private VfxType _vfxType;

        [Inject]
        private void Construct(VfxService vfxService)
        {
            _vfxService = vfxService;
        }

        private void Initialize(Vector3 position, Sprite sprite, EnemyViewModel context, VfxType vfxType,
            Action<Enemy> onDeath)
        {
            _viewModel = context;
            _vfxType = vfxType;
            transform.position = position;
            _onDeath = onDeath;
            _renderer.sprite = sprite;
            _collider.enabled = true;

            _collider.size = sprite.bounds.size;

            _viewModel.Position.Subscribe(pos => _rb.position = pos)
                .AddTo(_disposables);

            _viewModel.Rotation.Subscribe(rot => _rb.rotation = rot)
                .AddTo(_disposables);

            _viewModel.Velocity.Subscribe(vel => _rb.linearVelocity = vel)
                .AddTo(_disposables);

            _viewModel.OnDead.Subscribe(_ => _onDeath?.Invoke(this))
                .AddTo(_disposables);
        }

        public void Collide(ColliderType colliderType, int damage)
        {
            if (gameObject.activeInHierarchy) _viewModel.TakeCollision(colliderType, damage);
        }

        private void Update()
        {
            _viewModel.UpdatePosition();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.TryGetComponent(out ICollisionReceiver receiver)) return;

            _viewModel.MakeCollision(receiver);
            Debug.Log("Collided");
        }

        private void Despawn()
        {
            _vfxService.PlayVfx(_vfxType, transform.position);

            _rb.linearVelocity = Vector2.zero;
            _rb.angularVelocity = 0;
            gameObject.SetActive(false);
            _collider.enabled = false;
            _disposables.Dispose();
            _disposables = new CompositeDisposable();
        }

        public class Pool : MonoMemoryPool<Vector3, Sprite, EnemyViewModel,VfxType, Action<Enemy>, Enemy>
        {
            protected override void Reinitialize(Vector3 postion, Sprite sprite, EnemyViewModel context,
                VfxType vfxType, Action<Enemy> onDestroy, Enemy item)
            {
                item.Initialize(postion, sprite, context,vfxType, onDestroy);
            }

            protected override void OnDespawned(Enemy item)
            {
                item.Despawn();
            }
        }
    }
}
