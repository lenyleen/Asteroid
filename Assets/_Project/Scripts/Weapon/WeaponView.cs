using _Project.Scripts.Data;
using _Project.Scripts.Services;
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
        private VfxService _vfxService;
        private VfxType _vfxType;

        [Inject]
        private void Construct(VfxService vfxService, VfxType  vfxType)
        {
            _vfxService = vfxService;
            _vfxType = vfxType;
        }

        public void Initialize(WeaponViewModel viewModel, Sprite sprite)
        {
            _viewModel = viewModel;
            _spriteRenderer.sprite = sprite;

            _viewModel.OnShot.Subscribe(_ => _vfxService.PlayVfx(_vfxType, transform))
                .AddTo(this);
        }

        private void FixedUpdate()
        {
            _viewModel.Update();
        }
    }
}
