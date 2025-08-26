using _Project.Scripts.Data;
using _Project.Scripts.Pools;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Services
{
    public class VfxService
    {
        private readonly ParticlePool _particlePool;

        public VfxService(ParticlePool particlePool)
        {
            _particlePool = particlePool;
        }

        public void PlayVfx(VfxType vfxType, Transform parent, float rotation, float lifetime)
        {
            _particlePool.ShowParticle(vfxType, parent, rotation, lifetime);
        }
    }
}
