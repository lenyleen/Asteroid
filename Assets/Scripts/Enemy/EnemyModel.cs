using System;
using DataObjects;
using UniRx;
using UnityEngine;

namespace Enemy
{
    public class EnemyModel
    {
        public ReactiveProperty<int> Health { get; }
        public ReactiveCommand OnDeath { get; }
        public EnemyType Type => _data.Type;
        public ReactiveProperty<Vector3> Position;
        
        private readonly EnemyData _data;
        

        public EnemyModel(int health, EnemyData data, Vector3 position)
        {
            Health = new ReactiveProperty<int>(health) ;
            OnDeath = new ReactiveCommand();
            Position = new ReactiveProperty<Vector3>(position);
            _data = data;
        }
        public void TakeHit(int damage)
        {
            Health.Value -= damage;

            if (Health.Value <= 0)
                OnDeath.Execute();
        }
        
        public void UpdatePosition(Vector3 newPosition)
        {
            if (Position.Value == newPosition) return;

            Position.Value = newPosition;;
        }
    }
}