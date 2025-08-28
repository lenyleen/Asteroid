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
    [RequireComponent(typeof(AudioSource))]
    public class Enemy : MonoBehaviour, ICollisionReceiver
    {
        [field: SerializeField] public ColliderType ColliderType { get; private set; }

        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private float _maxSpeed;
        [SerializeField] private BoxCollider2D _collider;
        [SerializeField] private AudioSource _audioSource;

        private Vector2 _direction;
        private CompositeDisposable _disposables = new();
        private Action<Enemy> _onDeath;
        private EnemyViewModel _viewModel;
        private IVfxService  _vfxService;
        private VfxType _vfxType;

        [Inject]
        private void Construct(IVfxService vfxService)
        {
            _vfxService = vfxService;
        }

        private void Initialize(Vector3 position,EnemyInitData initData,
            Action<Enemy> onDeath)
        {
            _viewModel = initData.ViewModel;
            _vfxType = initData.VfxType;
            transform.position = position;
            _audioSource.clip = initData.AudioClip;

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

        private async void Despawn()
        {
            _vfxService.PlayVfx(_vfxType, transform.position);
            await _audioSource.PlayAsync();

            _rb.linearVelocity = Vector2.zero;
            _rb.angularVelocity = 0;
            gameObject.SetActive(false);
            _collider.enabled = false;
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
