using System;
using DataObjects;
using Interfaces;
using UniRx;
using UnityEngine;

namespace Player
{
    public class ShipViewModel : IPositionProvider, IPlayerStateNotifier
    {
        public ReadOnlyReactiveProperty<Vector3> Position { get; }
        public ReadOnlyReactiveProperty<Vector2> Velocity { get; }
        public ReadOnlyReactiveProperty<float> Rotation { get; } 
        public IObservable<Unit> OnDeath => _shipModel.OnDeath;
        
        private readonly ShipModel _shipModel;
        private readonly PlayerInputController _inputController;
        private readonly CompositeDisposable _disposables =  new ();
        
        public ShipViewModel(ShipModel shipModel, PlayerInputController playerInputController)
        {
            _shipModel = shipModel;
            _inputController = playerInputController;
            Position = new ReadOnlyReactiveProperty<Vector3>(_shipModel.Position);
            Velocity = new ReadOnlyReactiveProperty<Vector2>(_shipModel.Velocity);
            Rotation = new ReadOnlyReactiveProperty<float>(_shipModel.Rotation);
        }

        public void Initiialize()
        {
            _shipModel.OnDeath.Subscribe(_ => Dispose())
                .AddTo(_disposables);
        }
        
        public void Move(Vector2 direction)
        {
            _shipModel.UpdateMovement(direction);
            _shipModel.UpdateRotation(-direction.x);
        }

        public void TakeDamage(ColliderType colliderType, int damage)
        {
            _shipModel.TakeDamage(colliderType, damage);
        }
        
        private void Dispose()
        {
            Position.Dispose();
            Velocity.Dispose();
            Rotation.Dispose();
        }

        public void Update()
        {
            Move(_inputController.GetInputValues());
        }
    }
}