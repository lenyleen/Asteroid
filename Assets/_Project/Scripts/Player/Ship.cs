using _Project.Scripts.Configs;
using _Project.Scripts.Data;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Services;
using _Project.Scripts.Weapon;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
    public class Ship : MonoBehaviour, ICollisionReceiver
    {
        [field: SerializeField] public PlayerWeapons PlayerWeapons { get; private set; }
        [field: SerializeField] public ColliderType ColliderType { get; private set; }

        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private readonly CompositeDisposable _disposables = new();

        private ShipViewModel _shipViewModel;
        private VfxService  _vfxService;
        private VfxType  _vfxType;

        [Inject]
        private void Construct(VfxService vfxService)
        {
            _vfxService = vfxService;
        }

        public void Initialize(ShipViewModel shipViewModel, Sprite sprite, VfxType vfxType)
        {
            _shipViewModel = shipViewModel;

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

        public void OnDestroy()
        {
            _vfxService.PlayVfx(_vfxType,transform);

            _disposables.Dispose();
        }

        private void Update()
        {
            _shipViewModel.Update();
        }
    }
}
