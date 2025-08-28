using _Project.Scripts.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Factories
{
    public class BgmHandlerFactory
    {
        private readonly IProjectAssetProvider _projectAssetProvider;
        private readonly AudioMixerGroup _audioMixerGroup;

        public BgmHandlerFactory(IProjectAssetProvider projectAssetProvider, AudioMixerGroup audioMixerGroup)
        {
            _projectAssetProvider = projectAssetProvider;
            _audioMixerGroup = audioMixerGroup;
        }

        public async UniTask Create()
        {
            var soundAddress = SceneManager.GetActiveScene().name + "Audio";

            var audioSource = new GameObject("BgmHandler").AddComponent<AudioSource>();

            audioSource.outputAudioMixerGroup = _audioMixerGroup;

            var audio = await _projectAssetProvider.Load<AudioClip>(soundAddress);

            audioSource.clip = audio;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}
