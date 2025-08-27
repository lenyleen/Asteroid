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

        private ParticleSystem _mainParticleSystem;
        private Action<Particle> _onFinish;
        private Quaternion _startRotation;

        private void Awake()
        {
            _mainParticleSystem = GetComponent<ParticleSystem>();
            _startRotation = transform.rotation;
        }

        public void Display(Transform parent,Vector3 position,  Action<Particle> onFinish)
        {
            gameObject.SetActive(true);

            _onFinish = onFinish;
            transform.SetParent(parent,true);
            transform.localPosition = position;
            transform.localRotation = _startRotation;

            _mainParticleSystem.Play(true);
        }

        private void OnParticleSystemStopped() =>
            _onFinish?.Invoke(this);

        public void Deinitialize(Transform parent)
        {
            gameObject.SetActive(false);

            transform.SetParent(parent);
            transform.localPosition = Vector3.zero;

            _onFinish = null;
        }
    }
}
