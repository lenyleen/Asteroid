using System;
using DataObjects;
using Enemy.EnemyBehaviour;
using Interfaces;
using Signals;
using UniRx;
using UnityEngine;
using Zenject;

namespace Enemy
{
    public class EnemyViewModel :IInitializable, ISpawnableEnemy
    {
        private readonly EnemyModel _model;
        private readonly CompositeDisposable _disposables = new ();
        private readonly SignalBus _signalBus;
        private readonly IPositionProvider _positionProvider;
        public EnemyType  Type => _model.Type;
        public ReadOnlyReactiveProperty<float> Rotation { get; }
        public ReadOnlyReactiveProperty<Vector2> Velocity { get; }
        public ReadOnlyReactiveProperty<Vector3> Position { get; }
        public IObservable<Unit> OnDead => _model.OnDeath;
        
        public EnemyViewModel(EnemyModel model, SignalBus signalBus, IPositionProvider positionProvider)
        {
            _model = model;
            _positionProvider = positionProvider;
            Rotation = new ReadOnlyReactiveProperty<float>(_model.Rotation);
            Position = new ReadOnlyReactiveProperty<Vector3>(_model.Position);
            Velocity = new ReadOnlyReactiveProperty<Vector2>(_model.Velocity);
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            _model.OnDeath.Subscribe(_ => OnDeath())
                .AddTo(_disposables);

            _positionProvider.OnDeath.Take(1).Subscribe(_ 
                    => _model.OnNothingToAttack())
                .AddTo(_disposables);
        }
        public void MakeCollision(ICollisionReceiver collisionReceiver)
        {
            collisionReceiver.Collide(_model.ColliderType,_model.Damage);
        }

        public void TakeCollision(ColliderType colliderType, int damage)
        {
            _model.TakeHit(colliderType, damage);
        }

        private void OnLose(LoseSignal signal) => _model.OnNothingToAttack(); 
        
        public void UpdatePosition()
        {
            _model.UpdateMovement();
        }

        private void OnDeath()
        {
            _signalBus.Fire(new EnemyDestroyedSignal(_model.Type, _model.Score, _model.Position.Value));
            _disposables.Dispose();
        }

        ~EnemyViewModel()
        {
            Debug.Log($"Collected {this.GetType().Name} object");
        }
    }
}