using System;
using DG.Tweening;
using Interfaces;
using UniRx;
using UnityEngine;
using Zenject;

namespace Player
{
    public class Player : MonoBehaviour, IPositionMutator, IRotationProvider
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private float _friction;

        public Vector3 Position => transform.position;

        public float Rotation => transform.rotation.eulerAngles.z;
        public Vector2 Velocity => rb.linearVelocity;

        private readonly CompositeDisposable _disposables = new();
        
        private float _speed;
        private float _torque;

        [Inject]
        private void Constuct(PlayerViewModel playerViewModel)
        {
            playerViewModel._speed.Subscribe(speed => _speed = speed)
                .AddTo(_disposables);
            playerViewModel._torque.Subscribe(torque => _torque = torque)
                .AddTo(_disposables);
        }

        private void FixedUpdate()
        {
            Move(_speed);
            Rotate(_torque);
        }
        private void Move(float speed)
        {
            rb.AddRelativeForceY(speed);

            rb.linearVelocity *= _friction;
        }

        private void Rotate(float direction)
        {
            Debug.Log(direction);
            rb.MoveRotation(rb.rotation + direction);
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