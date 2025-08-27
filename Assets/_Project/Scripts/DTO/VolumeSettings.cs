using System;
using _Project.Scripts.Extensions;
using _Project.Scripts.Interfaces;
using UniRx;

namespace _Project.Scripts.DTO
{
    [Serializable]
    public class VolumeSettings : ILoadedData
    {
        private const float DefaultVolume = 1f;

        public ReadOnlyReactiveProperty<float> ReactiveSfxVolume => new (_reactiveSfxVolume);
        public ReadOnlyReactiveProperty<float> ReactiveMusicVolume => new (_reactiveMusicVolume);

        private float _sfxVolume;
        private float _musicVolume;

        [field:NonSerialized]private ReactiveProperty<float> _reactiveSfxVolume =  new ();
        [field:NonSerialized]private ReactiveProperty<float> _reactiveMusicVolume = new ();

        public VolumeSettings() =>
            SetVolumeSettings(DefaultVolume, DefaultVolume);

        public VolumeSettings(float sfxVolume, float musicVolume) =>
            SetVolumeSettings(sfxVolume, musicVolume);

        public void SetVolumeSettings(float sfxVolume, float musicVolume)
        {
            _sfxVolume = sfxVolume;
            _musicVolume = musicVolume;

            _reactiveSfxVolume.Value = sfxVolume;
            _reactiveMusicVolume.Value = musicVolume;
        }
    }
}
