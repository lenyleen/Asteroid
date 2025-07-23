using Configs;
using Interfaces;
using UniRx;
using UnityEngine;
using Weapon;

namespace Player
{
    public class Ship : MonoBehaviour, ICollisionReceiver
    {
        [field: SerializeField] public PlayerWeapons PlayerWeapons { get; private set; }

        [SerializeField] private Rigidbody2D rb;

        private readonly CompositeDisposable _disposables = new();

        private ShipViewModel _shipViewModel;

        private void Update()
        {
            _shipViewModel.Update();
        }

        public void OnDestroy()
        {
            _disposables.Dispose();
        }

        [field: SerializeField] public ColliderType ColliderType { get; private set; }

        public void Collide(ColliderType colliderType, int damage)
        {
            _shipViewModel.TakeDamage(colliderType, damage);
        }

        public void Initialize(ShipViewModel shipViewModel)
        {
            _shipViewModel = shipViewModel;

            _shipViewModel.Position.Subscribe(pos => rb.position = pos)
                .AddTo(_disposables);

            _shipViewModel.Rotation.Subscribe(rot => rb.rotation = rot)
                .AddTo(_disposables);

            _shipViewModel.Velocity.Subscribe(vel => rb.linearVelocity = vel)
                .AddTo(_disposables);

            _shipViewModel.OnDeath.Subscribe(_ => Destroy(gameObject))
                .AddTo(_disposables);
        }
    }
}
