using System;
using System.Collections.Generic;
using _Project.Scripts.Data;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Other
{
    [RequireComponent(typeof(ParticleSystem))]
    public class Particle : MonoBehaviour
    {
        [field: SerializeField] public VfxType VfxType { get; private set; }

        [SerializeField] private List<ParticleSystem> _particleSystems;

        private ParticleSystem _mainParticleSystem;
        private Action<Particle> _onFinish;

        private void Awake()
        {
            _mainParticleSystem = GetComponent<ParticleSystem>();
        }

        public void Display(Transform parent, float rotation, float lifetime, Action<Particle> onFinish)
        {
            foreach (var particleSystem in _particleSystems)
            {
                var main = particleSystem.main;
                main.startLifetime = lifetime;
            }

            gameObject.SetActive(true);

            _onFinish = onFinish;
            transform.SetParent(parent);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(0,0,rotation);

            _mainParticleSystem.Play(true);
        }

        private void OnParticleSystemStopped() =>
            _onFinish?.Invoke(this);

        public void Deinitialize(Transform parent)
        {
            gameObject.SetActive(false);

            transform.SetParent(parent);
            _onFinish = null;
        }
    }
}
