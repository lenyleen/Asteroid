using _Project.Scripts.Data;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Other;
using _Project.Scripts.Pools;
using UnityEngine;

namespace _Project.Scripts.Services
{
    public class FxService : IFxService
    {
        private readonly ParticlePool _particlePool;
        private readonly SoundPlayer.Pool _soundPlayerPool;

        public FxService(ParticlePool particlePool, SoundPlayer.Pool soundPlayerPool)
        {
            _particlePool = particlePool;
            _soundPlayerPool = soundPlayerPool;
        }

        public async void PlayVfx(VfxType vfxType,AudioClip audio, Transform parent)
        {
            _particlePool.ShowParticle(vfxType, parent, Vector3.zero);

            var soundPlayer = _soundPlayerPool.Spawn();
            await soundPlayer.Play(audio, parent.position);
        }

        public async void PlayVfx(VfxType vfxType, AudioClip audio, Vector3 position)
        {
            _particlePool.ShowParticle(vfxType,null, position);

            var soundPlayer = _soundPlayerPool.Spawn();
            await soundPlayer.Play(audio, position);
        }
    }
}
