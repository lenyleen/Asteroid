﻿using System;
using Configs;
using Interfaces;
using UniRx;
using UnityEngine;
using Zenject;

namespace Enemies
{
    public class EnemyViewModel : IInitializable, ISpawnableEnemy
    {
        private readonly CompositeDisposable _disposables = new();

        private readonly EnemyModel _model;
        private readonly IPositionProvider _positionProvider;

        public ReadOnlyReactiveProperty<float> Rotation { get; }
        public ReadOnlyReactiveProperty<Vector2> Velocity { get; }
        public IObservable<Unit> OnDead => _model.OnDeath;

        public EnemyType Type => _model.Type;
        public int Score => _model.Score;
        public ReadOnlyReactiveProperty<Vector3> Position { get; }

        public EnemyViewModel(EnemyModel model, IPositionProvider positionProvider)
        {
            _model = model;
            _positionProvider = positionProvider;
            Rotation = new ReadOnlyReactiveProperty<float>(_model.Rotation);
            Position = new ReadOnlyReactiveProperty<Vector3>(_model.Position);
            Velocity = new ReadOnlyReactiveProperty<Vector2>(_model.Velocity);
        }

        public void Initialize()
        {
            _model.OnDeath.Subscribe(_ => OnDeath())
                .AddTo(_disposables);

            _positionProvider.OnDeath.Take(1).Subscribe(_
                    => _model.Dispose())
                .AddTo(_disposables);
        }

        public void Despawn()
        {
            _model.Dispose();
        }

        public void MakeCollision(ICollisionReceiver collisionReceiver)
        {
            collisionReceiver.Collide(_model.ColliderType, _model.Damage);
        }

        public void TakeCollision(ColliderType colliderType, int damage)
        {
            _model.TakeHit(colliderType, damage);
        }

        public void UpdatePosition()
        {
            _model.UpdateMovement();
        }

        private void OnDeath()
        {
            _disposables.Dispose();
        }
    }
}
