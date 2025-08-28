using _Project.Scripts.Data;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Services;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Weapon
{
    [RequireComponent(typeof(SpriteRenderer),  typeof(AudioSource))]
    public class WeaponView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private AudioSource _audioSource;

        private WeaponViewModel _viewModel;
        private IVfxService _vfxService;
        private VfxType _vfxType;

        [Inject]
        private void Construct(IVfxService vfxService, VfxType  vfxType, AudioClip audioClip)
        {
            _vfxService = vfxService;
            _vfxType = vfxType;
            _audioSource.clip = audioClip;
        }

        public void Initialize(WeaponViewModel viewModel, Sprite sprite)
        {
            _viewModel = viewModel;
            _spriteRenderer.sprite = sprite;

            _viewModel.OnShot.Subscribe(_ => OnShot())
                .AddTo(this);
        }

        private void FixedUpdate()
        {
            _viewModel.Update();
        }

        private void OnShot()
        {
            _vfxService.PlayVfx(_vfxType, transform);
            _audioSource.Play();
        }
    }
}
