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
        public ColliderType ColliderType => _colliderData.colliderType;
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
            
        }

        public void UpdateLifetime(float deltaTime)
        {
            _lifetime -=  deltaTime;

            if (_lifetime < 0)
                OnDeath.Execute();
        }
    }
}