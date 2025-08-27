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
            _volumeSettingsProvider.VolumeSettings.ReactiveSfxVolume.Subscribe(vol =>
            {
                if (vol <= 0.05)
                    _sfxToggle.isOn = false;

                _sfxVolumeSlider.value = vol;
            }).AddTo(this);

            _volumeSettingsProvider.VolumeSettings.ReactiveMusicVolume.Subscribe(vol =>
            {
                if (vol <= 0.05)
                    _musicToggle.isOn = false;

                _musicVolumeSlider.value = vol;
            }).AddTo(this);

            _saveButton.OnClickAsObservable()
                .Subscribe(_ => SaveSettings())
                .AddTo(this);
        }

        private async void SaveSettings()
        {
            var sfxVolume = GetVolume(_sfxToggle, _sfxVolumeSlider);
            var musicVolume = GetVolume(_musicToggle, _musicVolumeSlider);

            await _volumeSettingsProvider.SaveVolumeSettings(sfxVolume, musicVolume);
            Hide();
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
