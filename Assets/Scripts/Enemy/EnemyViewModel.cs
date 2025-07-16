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
    public class EnemyViewModel : IEnemyContext, IInitializable
    {
        private readonly EnemyModel _model;
        private readonly CompositeDisposable _disposables = new ();
        private readonly SignalBus _signalBus;
        public ReactiveProperty<IEnemyBehaviour> Behaviour { get; }
        public ReactiveCommand OnDead => _model.OnDeath;
        
        public EnemyViewModel(EnemyModel model, SignalBus signalBus)
        {
            _model = model;
            Behaviour = new ReactiveProperty<IEnemyBehaviour>();
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            _model.OnDeath.Subscribe(_ => OnDeath())
                .AddTo(_disposables);
        }
        
        public void SetBehaviour(IEnemyBehaviour behaviour)
        {
            Behaviour.Value = behaviour;
        }

        public void TakeCollision(ICollisionReceiver collisionReceiver)
        {
            if(collisionReceiver.ColliderType == ColliderType.Enemy)
                return;
            
            _model.TakeHit(1);
        }
        
        public void UpdatePosition(Vector3 newPosition)
        {
            _model.UpdatePosition(newPosition);
        }

        private void OnDeath()
        {
            _signalBus.Fire(new EnemyDestroyedSignal(_model.Type, _model.Position.Value));
            _disposables.Dispose();
        }
    }
}