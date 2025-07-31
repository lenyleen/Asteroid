using System.Collections.Generic;
using _Project.Scripts.Configs;
using _Project.Scripts.Enemies.EnemyBehaviour;
using _Project.Scripts.Interfaces;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.Enemies
{
    public class EnemyModel
    {
        public ReactiveCommand OnDeath { get; }
        public EnemyType Type => _config.Type;
        public ColliderType ColliderType => _collisionConfig.ColliderType;
        public int Damage => _collisionConfig.Damage;
        public int Score { get; private set; }
        public ReactiveProperty<Vector3> Position { get; }
        public ReactiveProperty<Vector2> Velocity { get; } = new();
        public ReactiveProperty<float> Rotation { get; } = new();

        private readonly HashSet<ColliderType> _acceptableColliderTypes;
        private readonly ColliderConfig _collisionConfig;
        private readonly EnemyConfig _config;
        private readonly IPositionProvider _followingPositionProvider;
        private IEnemyBehaviour _behaviour;

        private int _health;

        public EnemyModel(EnemyConfig config, IEnemyBehaviour behaviour, Vector3 position,
            IPositionProvider followingPositionProvider)
        {
            _health = config.Health;
            OnDeath = new ReactiveCommand();
            Position = new ReactiveProperty<Vector3>(position);
            Score = config.Score;
            _followingPositionProvider = followingPositionProvider;
            _behaviour = behaviour;
            _config = config;
            _collisionConfig = config.CollisionConfig;
            _acceptableColliderTypes = new HashSet<ColliderType>(_collisionConfig.AcceptableColliderTypes);
        }

        public void TakeHit(ColliderType colliderType, int damage)
        {
            if (!_acceptableColliderTypes.Contains(colliderType))
                return;

            _health -= damage;

            if (_health > 0)
                return;

            if (colliderType == ColliderType.KillBox)
                Score = 0;

            OnDeath.Execute();
        }

        public void UpdateMovement()
        {
            if (_behaviour == null)
                return;

            var curPosition = Position.Value;
            var curRotation = Rotation.Value;
            var curVelocity = Velocity.Value;

            _behaviour.Update(ref curPosition, _followingPositionProvider.Position.Value, ref curVelocity,
                ref curRotation);

            Velocity.Value = curVelocity;
            Position.Value = curPosition;
            Rotation.Value = curRotation;
        }

        public void Dispose()
        {
            Score = 0;
            _behaviour = null;
            Velocity.Value = Vector3.zero;
            Position.Value = Vector3.zero;
            Rotation.Value = 0;
            OnDeath.Execute();
        }
    }
}
