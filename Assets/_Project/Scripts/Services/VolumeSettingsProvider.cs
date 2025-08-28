using System;
using _Project.Scripts.DTO;
using _Project.Scripts.Extensions;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Static;
using Cysharp.Threading.Tasks;
using UnityEngine.Audio;

namespace _Project.Scripts.Services
{
    public class VolumeSettingsProvider : ISceneInitializable
    {
        private const string MusicVolumeKey = "MusicVolume";
        private const string SfxVolumeKey = "SfxVolume";

        public VolumeSettings VolumeSettings { get; private set; }

        private readonly ISaveService _saveService;
        private readonly LocalSaveLoadService  _localSaveLoadService;
        private readonly AudioMixerGroup  _sfxMixer;
        private readonly AudioMixerGroup  _musicMixer;

        public VolumeSettingsProvider(ISaveService saveService, LocalSaveLoadService localSaveLoadService,
            AudioMixerGroup  sfxMixer, AudioMixerGroup  musicMixer)
        {
            _saveService = saveService;
            _localSaveLoadService = localSaveLoadService;
            _sfxMixer = sfxMixer;
            _musicMixer = musicMixer;
        }

        public async UniTask InitializeAsync()
        {
            var result = await _saveService.TryLoadData<VolumeSettings>(SaveKeys.VolumeSettingsKey);

            if(!result.Success)
                result = await _localSaveLoadService.TryLoadData<VolumeSettings>(SaveKeys.VolumeSettingsKey);

            VolumeSettings = result.Success ?  result.Data.LoadedData : new VolumeSettings();

            SetMixerVolume(VolumeSettings.SfxVolume,VolumeSettings.MusicVolume);
        }

        public async UniTask SaveVolumeSettings(float sfxVolume, float musicVolume)
        {
            VolumeSettings.SetVolumeSettings(sfxVolume, musicVolume);

            try
            {
                await _saveService.SaveData(VolumeSettings, SaveKeys.VolumeSettingsKey, DateTime.UtcNow);
            }
            catch (Exception)
            {
                await _localSaveLoadService.SaveData(VolumeSettings, SaveKeys.VolumeSettingsKey, DateTime.UtcNow);
            }

            SetMixerVolume(sfxVolume, musicVolume);
        }

        private void SetMixerVolume(float sfxVolume, float musicVolume)
        {

            _sfxMixer.audioMixer.SetFloat(SfxVolumeKey, sfxVolume.ConvertToDb());
            _musicMixer.audioMixer.SetFloat(MusicVolumeKey, musicVolume.ConvertToDb());
        }
    }
}
