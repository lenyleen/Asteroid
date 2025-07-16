using System;
using DG.Tweening;
using Interfaces;
using UniRx;
using UnityEngine;
using Weapon;
using Zenject;

namespace Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        [field: SerializeField] public PlayerWeapons PlayerWeapons { get; private set; }
        
        public Vector3 Position => transform.position;
        public float Rotation => transform.rotation.eulerAngles.z;
        
        private readonly CompositeDisposable _disposables = new();
        
        public void Construct(PlayerViewModel playerViewModel)
        {
            playerViewModel.Position.Subscribe(pos => rb.position = pos)
                .AddTo(_disposables);
                
            playerViewModel.Rotation.Subscribe(rot => rb.rotation = rot)
                .AddTo(_disposables);
                
            playerViewModel.Velocity.Subscribe(vel => rb.linearVelocity = vel)
                .AddTo(_disposables);
        }
        
        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}