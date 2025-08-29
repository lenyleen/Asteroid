using _Project.Scripts.Extensions;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Other
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;

        public async UniTask Play(AudioClip clip, Vector3 position)
        {
            transform.position = position;
            _audioSource.clip = clip;
            await _audioSource.PlayAsync();
        }

        private void Despawn()
        {
            _audioSource.Stop();
            _audioSource.clip = null;
            transform.position =  Vector3.zero;
        }

        public class Pool : MonoMemoryPool<SoundPlayer>
        {
            protected override void OnDespawned(SoundPlayer item)
            {

            }
        }
    }
}
