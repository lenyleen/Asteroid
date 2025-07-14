using Interfaces;
using UniRx;
using UnityEngine;

namespace Player
{
    public class PlayerViewModel: IMoveControllable
    {
        private readonly PlayerModel _playerModel;
        public ReactiveProperty<float> _speed = new ();
        public ReactiveProperty<float> _torque = new ();

        public PlayerViewModel(PlayerModel playerModel)
        {
            _playerModel = playerModel;
        }

        public void Move(Vector2 direction)
        {
            var movementY = Mathf.Max(0, direction.y);
            _speed.Value = movementY * _playerModel.Preferences.Acceleration;
            _torque.Value = -direction.x * _playerModel.Preferences.RotationSpeed * Time.fixedDeltaTime;
        }
        
        
    }
}