using Interfaces;
using UniRx;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerViewModel: IMoveControllable, IFixedTickable, IPositionProvider
    {
        private readonly PlayerModel _playerModel;
        private readonly PlayerInputController _inputController;
        public ReactiveProperty<Vector3> Position { get; } = new();
        public ReactiveProperty<Vector2> Velocity { get; } = new();
        public ReactiveProperty<float> Rotation { get; } = new();

        public PlayerViewModel(PlayerModel playerModel, PlayerInputController playerInputController)
        {
            _playerModel = playerModel;
            _inputController = playerInputController;
            
            _playerModel.OnPositionChanged += pos => Position.Value = pos;
            _playerModel.OnVelocityChanged += vel => Velocity.Value = vel;
            _playerModel.OnRotationChanged += rot => Rotation.Value = rot;
        }

        public void Move(Vector2 direction)
        {
            _playerModel.UpdateMovement(direction, Time.fixedDeltaTime);
            _playerModel.UpdateRotation(-direction.x, Time.fixedDeltaTime);
        }

        public void FixedTick()
        {
            Move(_inputController.GetInputValues());
        }
    }
}