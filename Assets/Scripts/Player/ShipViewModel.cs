using System;
using DataObjects;
using Interfaces;
using Signals;
using UniRx;
using UnityEngine;
using Zenject;

namespace Player
{
    public class ShipViewModel:IPositionProvider, IPlayerStateNotifier
    {
        private readonly ShipModel _shipModel;
        private readonly PlayerInputController _inputController;
        private readonly SignalBus _signalBus;
        private readonly CompositeDisposable _disposables =  new ();

        public ReadOnlyReactiveProperty<Vector3> Position { get; }
        public ReadOnlyReactiveProperty<Vector2> Velocity { get; }
        public ReadOnlyReactiveProperty<float> Rotation { get; } 
        public IObservable<Unit> OnDeath => _shipModel.OnDeath;
        
        public ShipViewModel(ShipModel shipModel, PlayerInputController playerInputController, SignalBus signalBus)
        {
            _shipModel = shipModel;
            _inputController = playerInputController;
            _signalBus   = signalBus;
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
        
        public void Dispose()
        {
            _signalBus.Fire<LoseSignal>();
            Position.Dispose();
            Velocity.Dispose();
            Rotation.Dispose();
        }

        public void Update()
        {
            Move(_inputController.GetInputValues());
        }

        ~ShipViewModel()
        {
            Debug.Log($"Collected {this.GetType().Name} object");
        }
    }
}