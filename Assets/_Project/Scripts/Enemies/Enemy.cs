using System;
using _Project.Scripts.Configs;
using _Project.Scripts.Data;
using _Project.Scripts.Extensions;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Other;
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
        private IFxService  _fxService;
        private VfxType _vfxType;
        private AudioClip _audioClip;

        [Inject]
        private void Construct(IFxService fxService)
        {
            _fxService = fxService;
        }

        private void Initialize(Vector3 position,EnemyInitData initData,
            Action<Enemy> onDeath)
        {
            _viewModel = initData.ViewModel;
            _vfxType = initData.VfxType;
            transform.position = position;
            _audioClip = initData.AudioClip;

            _onDeath = onDeath;
            _renderer.sprite = initData.Sprite;
            _collider.enabled = true;

            _collider.size = initData.Sprite.bounds.size;

            _viewModel.Position.Subscribe(pos => _rb.position = pos)
                .AddTo(_disposables);

            _viewModel.Rotation.Subscribe(rot => _rb.rotation = rot)
                .AddTo(_disposables);

            _viewModel.Velocity.Subscribe(vel => _rb.linearVelocity = vel)
                .AddTo(_disposables);

            _viewModel.OnDespawn.Subscribe(_ => _onDeath?.Invoke(this))
                .AddTo(_disposables);

            _viewModel.OnDead.Subscribe(_ => OnDeath())
                .AddTo(_disposables);
        }

        public void Collide(ColliderType colliderType, int damage)
        {
            if (gameObject.activeInHierarchy) _viewModel.TakeCollision(colliderType, damage);
        }

        private void Update() =>
            _viewModel.UpdatePosition();

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.TryGetComponent(out ICollisionReceiver receiver)) return;

            _viewModel.MakeCollision(receiver);
            Debug.Log("Collided");
        }

        private void OnDeath()
        {
            _fxService.PlayVfx(_vfxType,_audioClip, transform.position);
            _onDeath?.Invoke(this);
        }

        private void Despawn()
        {
            _collider.enabled = false;
            _rb.linearVelocity = Vector2.zero;
            _rb.angularVelocity = 0;
            gameObject.SetActive(false);
            _disposables.Dispose();
            _disposables = new CompositeDisposable();
        }

        public class Pool : MonoMemoryPool<Vector3, EnemyInitData, Action<Enemy>, Enemy>
        {
            protected override void Reinitialize(Vector3 postion, EnemyInitData initData, Action<Enemy> onDestroy, Enemy item)
            {
                item.Initialize(postion, initData, onDestroy);
            }

            protected override void OnDespawned(Enemy item)
            {
                item.Despawn();
            }
        }

        public class EnemyInitData
        {
            public Sprite Sprite { get; }
            public EnemyViewModel ViewModel { get; }
            public VfxType VfxType { get; }
            public AudioClip AudioClip { get; }

            public EnemyInitData(Sprite sprite, EnemyViewModel viewModel, VfxType vfxType, AudioClip audioClip)
            {
                Sprite = sprite;
                ViewModel = viewModel;
                VfxType = vfxType;
                AudioClip = audioClip;
            }
        }
    }
}
