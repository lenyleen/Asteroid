using _Project.Scripts.Interfaces;
using _Project.Scripts.Services;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.Scripts.UI.PopUps
{
    public class SettingsPopUp : PopUpBase, IUnParametrizedPopUp
    {
        [SerializeField] private Toggle _sfxToggle;
        [SerializeField] private Toggle _musicToggle;
        [SerializeField] private Slider _musicVolumeSlider;
        [SerializeField] private Slider _sfxVolumeSlider;
        [SerializeField] private Button _saveButton;

        private VolumeSettingsProvider _volumeSettingsProvider;

        [Inject]
        private void Construct(VolumeSettingsProvider volumeSettingsProvider)
        {
            _volumeSettingsProvider = volumeSettingsProvider;
        }

        public void Show()
        {
            SetVolume(_sfxToggle,_sfxVolumeSlider, _volumeSettingsProvider.VolumeSettings.SfxVolume);
            SetVolume(_musicToggle,_musicVolumeSlider, _volumeSettingsProvider.VolumeSettings.MusicVolume);

            _saveButton.OnClickAsObservable()
                .Subscribe(_ => SaveSettings())
                .AddTo(this);

            _closeButton.OnClickAsObservable().Subscribe(_ =>
                    Hide())
                .AddTo(this);
        }

        private async void SaveSettings()
        {
            var sfxVolume = GetVolume(_sfxToggle, _sfxVolumeSlider);
            var musicVolume = GetVolume(_musicToggle, _musicVolumeSlider);

            await _volumeSettingsProvider.SaveVolumeSettings(sfxVolume, musicVolume);
            Hide();
        }

        private void SetVolume(Toggle toggle, Slider slider, float vol)
        {
            if (vol <= 0.05)
                toggle.isOn = false;

            slider.value = vol;
        }

        private float GetVolume(Toggle toggle, Slider slider)
        {
            var volume = slider.value;

            if (toggle.isOn)
                volume = 0;

            return volume;
        }
    }
}
