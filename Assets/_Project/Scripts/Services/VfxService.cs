using _Project.Scripts.Data;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Pools;
using UnityEngine;

namespace _Project.Scripts.Services
{
    public class VfxService : IVfxService
    {
        private readonly ParticlePool _particlePool;

        public VfxService(ParticlePool particlePool)
        {
            _particlePool = particlePool;
        }

        public void PlayVfx(VfxType vfxType, Transform parent)
        {
            _particlePool.ShowParticle(vfxType, parent, Vector3.zero);
        }

        public void PlayVfx(VfxType vfxType, Vector3 position)
        {
            _particlePool.ShowParticle(vfxType,null, position);
        }
    }
}
