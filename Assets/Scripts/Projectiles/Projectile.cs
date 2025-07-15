using System;
using Interfaces;
using UniRx;
using UnityEngine;
using Zenject;

namespace Projectiles
{
    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
    public class Projectile : MonoBehaviour,IProjectile
    {
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private SpriteRenderer _renderer;
        
        private IProjectileBehaviour _projectileBehaviour;
        private Action<Projectile> _onDestroy;

        public void Initialize(Sprite sprite, Action<Projectile> onDestroy)
        {
            _renderer.sprite = sprite;
            _onDestroy = onDestroy;
            gameObject.SetActive(true);
        }
        
        public void ApplyParent(Transform transform)
        {
            transform.SetParent(transform);
            transform.position = Vector3.zero;
        }

        public void ApplyBehaviour(IProjectileBehaviour  projectileBehaviour)
        {
            _projectileBehaviour =  projectileBehaviour;
            _projectileBehaviour.OnDeath.Take(1)
                .Subscribe(_ => _onDestroy(this));
        }

        private void FixedUpdate()
        {
            if(_projectileBehaviour == null)
                return;
            
            _projectileBehaviour.Update(Time.fixedDeltaTime);
            var velocity = _projectileBehaviour.CalculateVelocity(transform.position);
            _rb.AddRelativeForce(velocity);
        }

        private void Despawn()
        {
            _projectileBehaviour = null;
            transform.SetParent(null);
            gameObject.SetActive(false);
            _renderer.sprite = null;
        }

        public class Pool : MonoMemoryPool<Sprite,Action<Projectile>,Projectile>
        {
            protected override void Reinitialize(Sprite sprite,Action<Projectile> onDestroy,Projectile item)
            {
                item.Initialize(sprite,onDestroy);
            }

            protected override void OnDespawned(Projectile item)
            {
                item.Despawn();
            }
        }
    }
}