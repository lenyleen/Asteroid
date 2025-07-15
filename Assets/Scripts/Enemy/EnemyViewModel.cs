using System;
using DataObjects;
using Enemy.EnemyBehaviour;
using Interfaces;
using UniRx;
using UnityEngine;
using Zenject;

namespace Enemy
{
    public class EnemyViewModel : IEnemyContext, IInitializable
    {
        private readonly EnemyModel _model;
        private readonly CompositeDisposable _disposables = new ();
        public ReactiveProperty<IEnemyBehaviour> Behaviour { get; }
        public ReactiveCommand OnDead => _model.OnDeath;
        
        public EnemyViewModel(EnemyModel model)
        {
            _model = model;
            Behaviour = new ReactiveProperty<IEnemyBehaviour>();
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

        private void OnDeath()
        {
            _disposables.Dispose();
        }
    }
}