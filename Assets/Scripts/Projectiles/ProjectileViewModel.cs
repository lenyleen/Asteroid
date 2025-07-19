using System;
using DataObjects;
using Interfaces;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using Unit = UniRx.Unit;

namespace Projectiles
{
    public class ProjectileViewModel : IInitializable
    {
        private readonly ProjectileModel _model;
        private readonly CompositeDisposable _disposables = new();
        
        public ReadOnlyReactiveProperty<Vector3> Position { get; }
        public ReadOnlyReactiveProperty<float> Rotation { get; }
        public ReadOnlyReactiveProperty<Vector2> Velocity { get; }
        public IObservable<Unit> OnDeath => _model.OnDeath;

        public ProjectileViewModel(ProjectileModel model)
        {
            _model = model;

            Position = new ReadOnlyReactiveProperty<Vector3>(_model.Position);
            Rotation = new ReadOnlyReactiveProperty<float>(_model.Rotation);
            Velocity = new ReadOnlyReactiveProperty<Vector2>(_model.Velocity);
        }

        public void Initialize()
        {
            _model.OnDeath.Subscribe(_ => OnDestroy())
                .AddTo(_disposables);
        }

        public void UpdatePosition()
        {
            _model.UpdateMovement();
        }

        public void UpdateLifeTime()
        {
            _model.UpdateLifetime();            
        }

        public void MakeCollision(ICollisionReceiver collisionReceiver)
        {
            if(collisionReceiver.ColliderType != ColliderType.Enemy)
                return;
            
            collisionReceiver.Collide(_model.ColliderType, _model.Damage);
            _model.TakeHit();
        }
        private void OnDestroy()
        {
            _disposables.Dispose();
        }

        ~ProjectileViewModel()
        {
            Debug.Log($"Collected {this.GetType().Name} object");
        }
    }
}