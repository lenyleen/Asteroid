using System;
using DataObjects;
using Interfaces;
using UniRx;
using UnityEngine;

namespace Projectiles
{
    public class ProjectileModel
    {
        public ReactiveProperty<Vector3> Position;
        public ReactiveProperty<float> Rotation;
        public ReactiveProperty<Vector2> Velocity;
        public ReactiveCommand OnDeath = new();
        public ColliderType ColliderType => _colliderData.ColliderType;
        public int Damage => _colliderData.Damage;
        
        private float _lifetime;
        
        private readonly ProjectileData  _data;
        private readonly ColliderData _colliderData;
        private readonly IProjectileBehaviour  _behaviour;

        public ProjectileModel(ProjectileData data, IProjectileBehaviour behaviour,
            Vector3 position, float rotation, Vector2 velocity )
        {
            Position = new ReactiveProperty<Vector3>(position);
            Rotation = new ReactiveProperty<float>(rotation);
            Velocity = new ReactiveProperty<Vector2>(velocity);
            
            _data =  data;
            _colliderData = data.ColliderData;
            _behaviour = behaviour;
            _lifetime = data.LifetimeInSeconds;
            
            _behaviour.Initialize(position, rotation);
        }

        public void UpdateMovement()
        {
            if (_behaviour == null) return;

            var curPosition = Position.Value;
            var curRotation = Rotation.Value;
            var curVelocity = Velocity.Value;
            
            _behaviour.Update(ref curPosition, ref curRotation, ref curVelocity);

            Rotation.Value = curRotation;
            Velocity.Value = curVelocity;
            Position.Value = curPosition;
        }

        public void TakeHit()
        {
            if (_behaviour.CheckDeathAfterCollision())
                OnDeath.Execute();
        }

        public void UpdateLifetime(float deltaTime = 0)
        {
            _lifetime -=  Time.deltaTime;

            if (_lifetime < 0)
                OnDeath.Execute();
        }
        ~ProjectileModel()
        {
            Debug.Log($"Collected {this.GetType().Name} object");
        }
    }
}