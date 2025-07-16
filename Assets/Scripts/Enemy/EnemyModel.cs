using System;
using DataObjects;
using Enemy.EnemyBehaviour;
using UniRx;
using UnityEngine;

namespace Enemy
{
    public class EnemyModel
    {
        public ReactiveProperty<int> Health { get; }
        public ReactiveCommand OnDeath { get; }
        public EnemyType Type => _data.Type;
        
        public ReactiveProperty<Vector3> Position { get; }
        public ReactiveProperty<Vector2> Velocity { get; } = new();
        public ReactiveProperty<float> Rotation { get; } = new();
        
        private readonly EnemyData _data;
        private IEnemyBehaviour _behaviour;
        
        
        public EnemyModel(int health, EnemyData data,IEnemyBehaviour behaviour, Vector3 position)
        {
            Health = new ReactiveProperty<int>(health) ;
            OnDeath = new ReactiveCommand();
            Position = new ReactiveProperty<Vector3>(position);
            _behaviour = behaviour;
            _data = data;
        }
        public void TakeHit(int damage)
        {
            Health.Value -= damage;

            if (Health.Value <= 0)
                OnDeath.Execute();
        }
        public void UpdateMovement()
        {
            if (_behaviour == null) return;
            
            
            var velocity = _behaviour.CalculateVelocity(Position.Value);
            var torque = _behaviour.CalculateTorque(Position.Value, Rotation.Value);
            
            Rotation.Value += torque * Time.deltaTime;
            Rotation.Value = Rotation.Value % 360f;

            Velocity.Value = velocity;
            
            if (Velocity.Value.magnitude > 10)
                Velocity.Value = Velocity.Value.normalized * 10;
            Position.Value += (Vector3)Velocity.Value * Time.deltaTime;
        }
    }
}