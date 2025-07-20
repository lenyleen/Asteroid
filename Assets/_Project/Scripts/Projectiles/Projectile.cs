using System;

using Interfaces;
using UniRx;
using UnityEngine;
using Zenject;

namespace Projectiles
{
    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(BoxCollider2D))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private BoxCollider2D _collider;

        private Action<Projectile> _onDestroy;
        private ProjectileViewModel _viewModel;
        private CompositeDisposable _disposable;
        
        public void Initialize(Sprite sprite,ProjectileViewModel viewModel,Action<Projectile> onDestroy)
        {
            _disposable = new CompositeDisposable();
            _renderer.sprite = sprite;
            _onDestroy = onDestroy;
            _viewModel = viewModel;
            _collider.enabled = true;
            _collider.size = sprite.bounds.size;
            _collider.offset = sprite.bounds.center;
            gameObject.SetActive(true);
            
            _viewModel.Position.Subscribe(pos => 
                _rb.position = pos)
                .AddTo(_disposable);
            
            _viewModel.Rotation.Subscribe(rot =>
                _rb.rotation = rot)
                .AddTo(_disposable);
            
            _viewModel.Velocity.Subscribe(v => 
                _rb.linearVelocity = v)
                .AddTo(_disposable);
            
            _viewModel.OnDeath.Subscribe(_ =>
                _onDestroy?.Invoke(this))
                .AddTo(_disposable);
        }

        private void Update()
        {
            _viewModel.UpdatePosition();
        }

        private void FixedUpdate()
        {
            _viewModel.UpdateLifeTime();
        }

        private void Despawn()
        {
            gameObject.SetActive(false);
            _renderer.sprite = null;
            _viewModel = null;
            _collider.enabled = false;
            _disposable.Dispose();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(!other.gameObject.TryGetComponent<ICollisionReceiver>(out var receiver))
                return;
            
            _viewModel.MakeCollision(receiver);
        }

        public class Pool : MonoMemoryPool<Sprite, ProjectileViewModel,Action<Projectile>,Projectile>
        {
            protected override void Reinitialize(Sprite sprite, ProjectileViewModel viewModel,Action<Projectile> onDestroy,Projectile item)
            {
                item.Initialize(sprite,viewModel,onDestroy);
            }

            protected override void OnDespawned(Projectile item)
            {
                item.Despawn();
            }
        }
    }
}