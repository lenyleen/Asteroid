using Interfaces;
using UniRx;
using UnityEngine;

namespace Projectiles
{
    public class LaserBehaviour : IProjectileBehaviour
    {
        private readonly IPositionProvider _shoterPositionProviderProvider;
        private readonly CompositeDisposable  _disposable = new ();
        
        private Vector3 _offsetFromShooter;
        private Vector3 _currentShooterPosition;
        private float _currentShooterRotation;
        
        public LaserBehaviour(IPositionProvider shoterPositionProvider) 
        {
            _shoterPositionProviderProvider = shoterPositionProvider;
            _currentShooterPosition = _shoterPositionProviderProvider.Position.Value;
            _currentShooterRotation = _shoterPositionProviderProvider.Rotation.Value;
        }
        
        public void Initialize(Vector3 spawnPosition, float shooterRotation)
        {
            var worldOffset = spawnPosition - _currentShooterPosition;
            _offsetFromShooter = Quaternion.Euler(0, 0, -shooterRotation) * worldOffset;
            
            _shoterPositionProviderProvider.Position.Subscribe(pos =>
                    _currentShooterPosition = pos)
                .AddTo(_disposable);

            _shoterPositionProviderProvider.Rotation.Subscribe(rot =>
                    _currentShooterRotation = rot)
                .AddTo(_disposable);
        }

        public void Update(ref Vector3 position, ref float rotation, ref Vector2 velocity)
        {
            var worldOffset = Quaternion.Euler(0, 0, _currentShooterRotation) * _offsetFromShooter;
            position = _currentShooterPosition + worldOffset;
            rotation = _currentShooterRotation;
            velocity = Vector2.zero;
        }

        public bool CheckDeathAfterCollision() => false;

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}