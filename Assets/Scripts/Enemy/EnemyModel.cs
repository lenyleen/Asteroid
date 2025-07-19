using System;
using System.Collections.Generic;
using DataObjects;
using Enemy.EnemyBehaviour;
using Interfaces;
using UniRx;
using UnityEngine;

namespace Enemy
{
    public class EnemyModel
    {
        public ReactiveProperty<int> Health { get; }
        public ReactiveCommand OnDeath { get; }
        public EnemyType Type => _data.Type;
        public ColliderType ColliderType => _collisionData.ColliderType;
        public int Damage => _collisionData.Damage;
        public int Score { get; private set; }
        public ReactiveProperty<Vector3> Position { get; }
        public ReactiveProperty<Vector2> Velocity { get; } = new();
        public ReactiveProperty<float> Rotation { get; } = new();
        
        private readonly EnemyData _data;
        private readonly IPositionProvider _followingPositionProvider;
        private readonly ColliderData _collisionData;
        private readonly HashSet<ColliderType>  _acceptableColliderTypes;
        private IEnemyBehaviour _behaviour;
        
        
        public EnemyModel(EnemyData data,IEnemyBehaviour behaviour, Vector3 position, 
            IPositionProvider followingPositionProvider)
        {
            Health = new ReactiveProperty<int>(data.Health) ;
            OnDeath = new ReactiveCommand();
            Position = new ReactiveProperty<Vector3>(position);
            Score = data.Score;
            _followingPositionProvider = followingPositionProvider;
            _behaviour = behaviour;
            _data = data;
            _collisionData = data.CollisionData;
            _acceptableColliderTypes = new HashSet<ColliderType>(_collisionData.AcceptableColliderTypes);
        }
        public void TakeHit(ColliderType colliderType, int damage)
        {  
            if(!_acceptableColliderTypes.Contains(colliderType))
                return;

            Health.Value -= damage;
            
            if (Health.Value > 0) return;
            
            if(colliderType == ColliderType.KillBox)
                Score = 0;
            OnDeath.Execute();
        }
        public void UpdateMovement()
        {
            if (_behaviour == null) return;
            
            var curPosition = Position.Value;
            var curRotation = Rotation.Value;
            var curVelocity = Velocity.Value;
            
            _behaviour.Update(ref curPosition,_followingPositionProvider.Position.Value,ref curVelocity , ref curRotation);
            
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
        ~EnemyModel()
        {
            Debug.Log($"Collected {this.GetType().Name} object");
        }
    }
}