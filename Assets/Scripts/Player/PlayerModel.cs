using System;
using DataObjects;
using Interfaces;
using Services;
using UnityEngine;

namespace Player
{
    public class PlayerModel
    {
        public  PlayerPreferences Preferences => _playerPreferences;
        private readonly PlayerPreferences _playerPreferences;

        public Vector2 Position { get; private set; }
        public Vector2 Velocity { get; private set; }
        public float Rotation { get; private set; }
        
        public event Action<Vector2> OnPositionChanged;
        public event Action<Vector2> OnVelocityChanged;
        public event Action<float> OnRotationChanged;

        public PlayerModel(PlayerPreferences playerPreferences)
        {
            _playerPreferences = playerPreferences;
        }
        
        public void UpdateMovement(Vector2 input, float deltaTime)
        {
            Vector2 acceleration = Vector2.zero;
            if (input.y > 0)
            {
                float angle = -Rotation * Mathf.Deg2Rad;
                acceleration = new Vector2(
                    Mathf.Sin(angle) * _playerPreferences.Acceleration,
                    Mathf.Cos(angle) * _playerPreferences.Acceleration
                );
            }
            
            Velocity += acceleration * deltaTime;
            
            
            if (Velocity.magnitude > _playerPreferences.MaxSpeed)
                Velocity = Velocity.normalized * _playerPreferences.MaxSpeed;
            
            Velocity *= _playerPreferences.Friction;
            
            Position += Velocity * deltaTime;

            Position = new ScreenWrapService(Camera.main).GetInScreenPosition(Position);
            
            OnVelocityChanged?.Invoke(Velocity);
            OnPositionChanged?.Invoke(Position);
        }
        
        public void UpdateRotation(float input, float deltaTime)
        {
            if (Mathf.Abs(input) > 0.1f)
            {
                float rotationDelta = input * _playerPreferences.RotationSpeed * deltaTime;
                Rotation += rotationDelta;
                
                Rotation = Rotation % 360f;
                if (Rotation < 0) Rotation += 360f;
                
                OnRotationChanged?.Invoke(Rotation);
            }
        }
    }
}
