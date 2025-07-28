using System;
using System.Collections.Generic;
using Configs;
using Services;
using UniRx;
using UnityEngine;

namespace Player
{
    public class ShipModel
    {
        public IObservable<Unit> OnDeath => _deathCommand;
        public IObservable<Vector3> Position => _position;
        public IObservable<float> Rotation => _rotation;
        public IObservable<Vector2> Velocity => _velocity;

        private readonly ReactiveProperty<Vector3> _position = new(Vector2.zero);
        private readonly ReactiveProperty<float> _rotation = new(0);
        private readonly ReactiveProperty<Vector2> _velocity = new(Vector2.zero);
        private readonly ReactiveCommand _deathCommand = new();

        private readonly HashSet<ColliderType> _acceptableColliderTypes;
        private readonly ScreenWrapService _screenWrapService;
        private readonly ShipPreferences _shipPreferences;

        private int _health;

        public ShipModel(ShipPreferences shipPreferences, ScreenWrapService screenWrapService)
        {
            _shipPreferences = shipPreferences;
            _health = shipPreferences.Health;
            _screenWrapService = screenWrapService;
            var colliderConfig = shipPreferences.ColliderConfig;
            _acceptableColliderTypes = new HashSet<ColliderType>(colliderConfig.AcceptableColliderTypes);
        }

        public void TakeDamage(ColliderType colliderType, int damage)
        {
            if (!_acceptableColliderTypes.Contains(colliderType))
                return;

            _health -= damage;
            if (_health <= 0)
                _deathCommand.Execute();
        }

        public void UpdateMovement(Vector2 input)
        {
            var acceleration = Vector2.zero;
            if (input.y > 0)
            {
                var angle = -_rotation.Value * Mathf.Deg2Rad;
                acceleration = new Vector2(
                    Mathf.Sin(angle) * _shipPreferences.Acceleration,
                    Mathf.Cos(angle) * _shipPreferences.Acceleration
                );
            }

            _velocity.Value += acceleration * Time.deltaTime;

            if (_velocity.Value.magnitude > _shipPreferences.MaxSpeed)
                _velocity.Value = _velocity.Value.normalized * _shipPreferences.MaxSpeed;

            _velocity.Value *= _shipPreferences.Friction;
            _position.Value += (Vector3)_velocity.Value * Time.deltaTime;
            _position.Value = _screenWrapService.GetInScreenPosition(_position.Value);
        }

        public void UpdateRotation(float input)
        {
            if (!(Mathf.Abs(input) > 0.1f))
                return;

            var rotationDelta = input * _shipPreferences.RotationSpeed * Time.deltaTime;
            _rotation.Value += rotationDelta;

            _rotation.Value %= 360f;
            if (_rotation.Value < 0)
                _rotation.Value += 360f;
        }
    }
}
