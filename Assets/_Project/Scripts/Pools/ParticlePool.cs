using System.Collections.Generic;
using _Project.Scripts.Data;
using _Project.Scripts.Factories;
using _Project.Scripts.Other;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Pools
{
    public class ParticlePool
    {
        private readonly Transform _poolParent;
        private readonly ParticleFactory  _particleFactory;
        private readonly int _increasingStep;
        private readonly Dictionary<VfxType, Stack<Particle>> _pool = new();

        public ParticlePool(Transform poolParent, ParticleFactory particleFactory, int poolInitialSize,
            int increasingStep = 3)
        {
            _poolParent = poolParent;
            _particleFactory = particleFactory;
        }

        public async void ShowParticle(VfxType vfxType, Transform parent, float rotation, float lifetime)
        {
            if(!_pool.TryGetValue(vfxType, out var particles))
                _pool.Add(vfxType, particles = new Stack<Particle>());

            if (!particles.TryPop(out var particle))
                particle = await CreateParticles(vfxType, particles);

            particle.Display(parent, rotation, lifetime,Return);
        }

        private async UniTask<Particle> CreateParticles(VfxType vfxType, Stack<Particle> particles)
        {
            var newParticles = await _particleFactory.Create(vfxType,_poolParent, _increasingStep);

            foreach (var particle in newParticles)
                particles.Push(particle);

            return particles.Pop();
        }

        private void Return(Particle particle)
        {
            if(!_pool.TryGetValue(particle.VfxType, out var particles))
            {
                Debug.LogWarning("Particle pool removed during gameplay!");
                return;
            }

            particle.Deinitialize(_poolParent);

            particles.Push(particle);
        }
    }
}
