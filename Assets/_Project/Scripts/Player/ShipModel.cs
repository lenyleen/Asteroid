using System.Collections.Generic;
using Configs;
using Services;
using UniRx;
using UnityEngine;

namespace Player
{
    public class ShipModel
    {
        private readonly HashSet<ColliderType> _acceptableColliderTypes;
        private readonly ScreenWrapService _screenWrapService;

        private readonly ShipPreferences _shipPreferences;

        private int _health;

        public ReactiveProperty<Vector3> Position { get; } = new(Vector2.zero);
        public ReactiveProperty<float> Rotation { get; } = new(0);
        public ReactiveProperty<Vector2> Velocity { get; } = new(Vector2.zero);
        public ReactiveCommand OnDeath { get; } = new();

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
            {
                return;
            }

            _health -= damage;
            if (_health <= 0)
            {
                OnDeath.Execute();
            }
        }

        public void UpdateMovement(Vector2 input)
        {
            var acceleration = Vector2.zero;
            if (input.y > 0)
            {
                var angle = -Rotation.Value * Mathf.Deg2Rad;
                acceleration = new Vector2(
                    Mathf.Sin(angle) * _shipPreferences.Acceleration,
                    Mathf.Cos(angle) * _shipPreferences.Acceleration
                );
            }

            Velocity.Value += acceleration * Time.deltaTime;

            if (Velocity.Value.magnitude > _shipPreferences.MaxSpeed)
            {
                Velocity.Value = Velocity.Value.normalized * _shipPreferences.MaxSpeed;
            }

            Velocity.Value *= _shipPreferences.Friction;
            Position.Value += (Vector3)Velocity.Value * Time.deltaTime;
            Position.Value = _screenWrapService.GetInScreenPosition(Position.Value);
        }

        public void UpdateRotation(float input)
        {
            if (!(Mathf.Abs(input) > 0.1f))
            {
                return;
            }

            var rotationDelta = input * _shipPreferences.RotationSpeed * Time.deltaTime;
            Rotation.Value += rotationDelta;

            Rotation.Value %= 360f;
            if (Rotation.Value < 0)
            {
                Rotation.Value += 360f;
            }
        }
    }
}
