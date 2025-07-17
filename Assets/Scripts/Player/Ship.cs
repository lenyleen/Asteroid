using System;
using DataObjects;
using DG.Tweening;
using Interfaces;
using UniRx;
using UnityEngine;
using Weapon;
using Zenject;

namespace Player
{
    public class Ship : MonoBehaviour, ICollisionReceiver
    {
        [SerializeField] private Rigidbody2D rb;
        [field: SerializeField] public PlayerWeapons PlayerWeapons { get; private set; }
        [field:SerializeField]public ColliderType ColliderType { get; private set; }
        
        private ShipViewModel _shipViewModel; 
        
        private readonly CompositeDisposable _disposables = new();
        
        public void Construct(ShipViewModel shipViewModel)
        {
            _shipViewModel = shipViewModel;
            
            _shipViewModel.Position.Subscribe(pos => rb.position = pos)
                .AddTo(_disposables);
                
            _shipViewModel.Rotation.Subscribe(rot => rb.rotation = rot)
                .AddTo(_disposables);
                
            _shipViewModel.Velocity.Subscribe(vel => rb.linearVelocity = vel)
                .AddTo(_disposables);
            
            _shipViewModel.OnDeath.Subscribe(_ => Destroy(this.gameObject))
                .AddTo(_disposables);
        }

        private void FixedUpdate()
        {
            _shipViewModel.Update();
        }

        public void OnDestroy()
        {
            _disposables.Dispose();
        }

        
        public void Collide(ICollisionReceiver collisionReceiver)
        {
            _shipViewModel.TakeDamage();
        }
    }
}