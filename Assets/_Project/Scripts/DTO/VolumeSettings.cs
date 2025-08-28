using System;
using _Project.Scripts.Interfaces;
using Newtonsoft.Json;

namespace _Project.Scripts.DTO
{
    [Serializable]
    public class VolumeSettings : ILoadedData
    {
        private const float DefaultVolume = 1f;

        [JsonProperty]public float SfxVolume { get; private set; }
        [JsonProperty]public float MusicVolume{get; private set;}

        public VolumeSettings() =>
            SetVolumeSettings(DefaultVolume, DefaultVolume);

        [JsonConstructor]
        public VolumeSettings(float SfxVolume, float MusicVolume) =>
            SetVolumeSettings(SfxVolume, MusicVolume);

        public void SetVolumeSettings(float sfxVolume, float musicVolume)
        {
            SfxVolume = sfxVolume;
            MusicVolume = musicVolume;
        }
    }
}
