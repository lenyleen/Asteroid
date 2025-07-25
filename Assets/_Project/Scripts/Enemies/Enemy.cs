﻿using System;
using Configs;
using Interfaces;
using UniRx;
using UnityEngine;
using Zenject;

namespace Enemies
{
    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(BoxCollider2D))]
    public class Enemy : MonoBehaviour, ICollisionReceiver
    {
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private float _maxSpeed;
        [SerializeField] private BoxCollider2D _collider;
        private Vector2 _direction;
        private CompositeDisposable _disposables = new();
        private Action<Enemy> _onDeath;

        private EnemyViewModel _viewModel;

        private void Update()
        {
            _viewModel.UpdatePosition();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.TryGetComponent(out ICollisionReceiver receiver))
            {
                return;
            }

            _viewModel.MakeCollision(receiver);
            Debug.Log("Collided");
        }

        [field: SerializeField] public ColliderType ColliderType { get; private set; }

        public void Collide(ColliderType colliderType, int damage)
        {
            if (gameObject.activeInHierarchy)
            {
                _viewModel.TakeCollision(colliderType, damage);
            }
        }

        private void Initialize(Vector3 position, Sprite sprite, EnemyViewModel context, Action<Enemy> onDeath)
        {
            _viewModel = context;
            transform.position = position;
            _onDeath = onDeath;
            _renderer.sprite = sprite;
            _collider.enabled = true;

            _collider.size = sprite.bounds.size;

            _viewModel.Position.Subscribe(pos => _rb.position = pos)
                .AddTo(_disposables);

            _viewModel.Rotation.Subscribe(rot => _rb.rotation = rot)
                .AddTo(_disposables);

            _viewModel.Velocity.Subscribe(vel => _rb.linearVelocity = vel)
                .AddTo(_disposables);

            _viewModel.OnDead.Subscribe(_ => _onDeath?.Invoke(this))
                .AddTo(_disposables);
        }

        private void Despawn()
        {
            _rb.linearVelocity = Vector2.zero;
            _rb.angularVelocity = 0;
            gameObject.SetActive(false);
            _collider.enabled = false;
            _disposables.Dispose();
            _disposables = new CompositeDisposable();
        }

        public class Pool : MonoMemoryPool<Vector3, Sprite, EnemyViewModel, Action<Enemy>, Enemy>
        {
            protected override void Reinitialize(Vector3 postion, Sprite sprite, EnemyViewModel context,
                Action<Enemy> onDestroy, Enemy item)
            {
                item.Initialize(postion, sprite, context, onDestroy);
            }

            protected override void OnDespawned(Enemy item)
            {
                item.Despawn();
            }
        }
    }
}
