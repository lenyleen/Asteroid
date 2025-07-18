using System;
using System.Collections.Generic;
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
        public ReactiveProperty<Vector3> Position { get; } = new (Vector2.zero);
        public ReactiveProperty<float> Rotation { get; } = new(0);
        public ReactiveProperty<Vector2> Velocity { get; } = new (Vector2.zero);
        public ReactiveCommand OnDeath { get; } = new ();
        
        private readonly PlayerPreferences _playerPreferences;
        private readonly ScreenWrapService _screenWrapService;
        private readonly ColliderData  _colliderData;
        private readonly HashSet<ColliderType> _acceptableColliderTypes;
        
        private int _health;

        public ShipModel(PlayerPreferences playerPreferences, ScreenWrapService screenWrapService)
        {
            _playerPreferences = playerPreferences;
            _health = playerPreferences.Health;
            _screenWrapService = screenWrapService;
            _colliderData = playerPreferences.ColliderData;
            _acceptableColliderTypes = new HashSet<ColliderType>(_colliderData.AcceptableColliderTypes);
        }

        public void TakeDamage(ColliderType colliderType, int damage)
        {
            if(!_acceptableColliderTypes.Contains(colliderType))
                return;

            _health -= damage;
            if(_health <= 0)
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

        ~ShipModel()
        {
            Debug.Log($"Collected {this.GetType().Name} object");
        }
    }
}
