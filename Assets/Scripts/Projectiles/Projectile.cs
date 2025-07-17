using System;
using DataObjects;
using Interfaces;
using UniRx;
using UnityEngine;
using Zenject;

namespace Projectiles
{
    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(BoxCollider2D))]
    public class Projectile : MonoBehaviour,IProjectile, ICollisionReceiver
    {
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private BoxCollider2D _collider;
        public ColliderType ColliderType { get; }
        

        private IProjectileBehaviour _projectileBehaviour;
        private Action<Projectile> _onDestroy;

        public void Initialize(Sprite sprite,float rotation, Action<Projectile> onDestroy)
        {
            _renderer.sprite = sprite;
            _collider.size = sprite.bounds.size;
            _collider.offset = sprite.bounds.center;
            _onDestroy = onDestroy;
            transform.rotation = Quaternion.Euler(0, 0, rotation);
            gameObject.SetActive(true);
        }
        
        public void ApplyParent(Transform transform)
        {
            transform.SetParent(transform);
            transform.localPosition = Vector3.zero;
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
            
            if(!gameObject.activeInHierarchy)
                return;
            
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
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(!other.gameObject.TryGetComponent<ICollisionReceiver>(out var receiver))
                return;
             
            if(receiver.ColliderType != ColliderType.Enemy)
                return;

            receiver.Collide(this);
            Debug.Log("colleded");
        }
        public void Collide(ICollisionReceiver collisionReceiver)
        {
            _projectileBehaviour.Collided();
        }

        public class Pool : MonoMemoryPool<Sprite,float,Action<Projectile>,Projectile>
        {
            protected override void Reinitialize(Sprite sprite, float rotation,Action<Projectile> onDestroy,Projectile item)
            {
                item.Initialize(sprite,rotation,onDestroy);
            }

            protected override void OnDespawned(Projectile item)
            {
                item.Despawn();
            }
        }
    }
}