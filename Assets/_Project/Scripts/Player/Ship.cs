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
    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
    public class Ship : MonoBehaviour, ICollisionReceiver
    {
        [field: SerializeField] public PlayerWeapons PlayerWeapons { get; private set; }
        [field: SerializeField] public ColliderType ColliderType { get; private set; }

        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private ShipViewModel _shipViewModel;
        private IFxService  _fxService;
        private VfxType  _vfxType;
        private AudioClip _audio;

        [Inject]
        private void Construct(IFxService fxService)
        {
            _fxService = fxService;
        }

        public void Initialize(ShipViewModel shipViewModel, Sprite sprite, VfxType vfxType, AudioClip audioClip)
        {
            _shipViewModel = shipViewModel;
            _audio = audioClip;

            _spriteRenderer.sprite = sprite;

            _vfxType = vfxType;

            _shipViewModel.Position.Subscribe(pos => _rb.position = pos)
                .AddTo(this);

            _shipViewModel.Rotation.Subscribe(rot => _rb.rotation = rot)
                .AddTo(this);

            _shipViewModel.Velocity.Subscribe(vel => _rb.linearVelocity = vel)
                .AddTo(this);

            _shipViewModel.OnDeath.Subscribe(_ => OnDeath())
                .AddTo(this);
        }

        public void Collide(ColliderType colliderType, int damage)
        {
            _shipViewModel.TakeDamage(colliderType, damage);
        }

        private void OnDeath()
        {
            Destroy(gameObject);
        }

        public void OnDestroy()
        {
            _fxService.PlayVfx(_vfxType,_audio,transform.position);
        }

        private void Update()
        {
            _shipViewModel.Update();
        }
    }
}
