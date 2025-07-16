using Interfaces;
using UniRx;
using UnityEngine;
using Zenject;

namespace Player
{
    public class ShipViewModel: IMoveControllable, IFixedTickable, IPositionProvider
    {
        private readonly ShipModel _shipModel;
        private readonly PlayerInputController _inputController;
        public ReactiveProperty<Vector3> Position { get; } = new();
        public ReactiveProperty<Vector2> Velocity { get; } = new();
        public ReactiveProperty<float> Rotation { get; } = new();

        public ShipViewModel(ShipModel shipModel, PlayerInputController playerInputController)
        {
            _shipModel = shipModel;
            _inputController = playerInputController;
            
            _shipModel.OnPositionChanged += pos => Position.Value = pos;
            _shipModel.OnVelocityChanged += vel => Velocity.Value = vel;
            _shipModel.OnRotationChanged += rot => Rotation.Value = rot;
        }

        public void Move(Vector2 direction)
        {
            _shipModel.UpdateMovement(direction, Time.fixedDeltaTime);
            _shipModel.UpdateRotation(-direction.x, Time.fixedDeltaTime);
        }

        public void FixedTick()
        {
            Move(_inputController.GetInputValues());
        }
    }
}