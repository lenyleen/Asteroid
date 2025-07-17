using System;
using DataObjects;
using Interfaces;
using JetBrains.Annotations;
using Services;
using UniRx;
using UnityEngine;

namespace Player
{
    public class ShipModel
    {
        public  PlayerPreferences Preferences => _playerPreferences;
        public ReactiveProperty<Vector3> Position { get; } = new (Vector2.zero);
        public ReactiveProperty<float> Rotation { get; } = new(0);
        public ReactiveProperty<Vector2> Velocity { get; } = new (Vector2.zero);
        public ReactiveCommand OnDeath { get; } = new ();
        
        private readonly PlayerPreferences _playerPreferences;
        private readonly ScreenWrapService _screenWrapService;
        
        private int _health;

        public ShipModel(PlayerPreferences playerPreferences, ScreenWrapService screenWrapService)
        {
            _playerPreferences = playerPreferences;
            _health = playerPreferences.Health;
            _screenWrapService = screenWrapService;
        }

        public void TakeDamage()
        {
            Velocity.Value = Vector2.zero;
            OnDeath.Execute();
        }
        
        public void UpdateMovement(Vector2 input, float deltaTime)
        {
            Vector2 acceleration = Vector2.zero;
            if (input.y > 0)
            {
                float angle = -Rotation.Value * Mathf.Deg2Rad;
                acceleration = new Vector2(
                    Mathf.Sin(angle) * _playerPreferences.Acceleration,
                    Mathf.Cos(angle) * _playerPreferences.Acceleration
                );
            }
            
            Velocity.Value += acceleration * deltaTime;
            
            
            if (Velocity.Value.magnitude > _playerPreferences.MaxSpeed)
                Velocity.Value = Velocity.Value.normalized * _playerPreferences.MaxSpeed;
            
            Velocity.Value *= _playerPreferences.Friction;
            
            Position.Value += (Vector3)Velocity.Value * deltaTime;

            Position.Value = _screenWrapService.GetInScreenPosition(Position.Value);
            
        }
        
        public void UpdateRotation(float input, float deltaTime)
        {
            if (Mathf.Abs(input) > 0.1f)
            {
                float rotationDelta = input * _playerPreferences.RotationSpeed * deltaTime;
                Rotation.Value += rotationDelta;
                
                Rotation.Value %=  360f;
                if (Rotation.Value < 0) Rotation.Value += 360f;
            }
        }
    }
}
