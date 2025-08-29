using _Project.Scripts.Data;
using _Project.Scripts.Interfaces;
using UniRx;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Weapon
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class WeaponView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private WeaponViewModel _viewModel;
        private IFxService _fxService;
        private VfxType _vfxType;
        private AudioClip _audioClip;

        [Inject]
        private void Construct(IFxService fxService, VfxType  vfxType, AudioClip audioClip)
        {
            _fxService = fxService;
            _vfxType = vfxType;
            _audioClip = audioClip;
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
            if(!isActiveAndEnabled)
                return;

            _fxService.PlayVfx(_vfxType,_audioClip, transform);
        }
    }
}
