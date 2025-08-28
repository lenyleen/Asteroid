using _Project.Scripts.Configs;
using _Project.Scripts.Data;
using _Project.Scripts.Extensions;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Weapon;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(AudioSource))]
    public class Ship : MonoBehaviour, ICollisionReceiver
    {
        [field: SerializeField] public PlayerWeapons PlayerWeapons { get; private set; }
        [field: SerializeField] public ColliderType ColliderType { get; private set; }

        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private AudioSource _audioSource;

        private readonly CompositeDisposable _disposables = new();

        private ShipViewModel _shipViewModel;
        private IVfxService  _vfxService;
        private VfxType  _vfxType;

        [Inject]
        private void Construct(IVfxService vfxService)
        {
            _vfxService = vfxService;
        }

        public void Initialize(ShipViewModel shipViewModel, Sprite sprite, VfxType vfxType, AudioClip audioClip)
        {
            _shipViewModel = shipViewModel;
            _audioSource.clip = audioClip;

            _spriteRenderer.sprite = sprite;

            _vfxType = vfxType;

            _shipViewModel.Position.Subscribe(pos => _rb.position = pos)
                .AddTo(_disposables);

            _shipViewModel.Rotation.Subscribe(rot => _rb.rotation = rot)
                .AddTo(_disposables);

            _shipViewModel.Velocity.Subscribe(vel => _rb.linearVelocity = vel)
                .AddTo(_disposables);

            _shipViewModel.OnDeath.Subscribe(_ => Destroy(gameObject))
                .AddTo(_disposables);
        }

        public void Collide(ColliderType colliderType, int damage)
        {
            _shipViewModel.TakeDamage(colliderType, damage);
        }

        public async void OnDestroy()
        {
            _vfxService.PlayVfx(_vfxType,transform);
            await _audioSource.PlayAsync();

            _disposables.Dispose();
        }

        private void Update()
        {
            _shipViewModel.Update();
        }
    }
}
