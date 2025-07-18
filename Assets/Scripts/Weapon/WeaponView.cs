using System;
using Interfaces;
using Projectiles;
using UniRx;
using UnityEngine;

namespace Weapon
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class WeaponView : MonoBehaviour , IWeapon
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        private  WeaponViewModel _viewModel;
        private readonly CompositeDisposable _disposables = new ();
        public void Initialize(WeaponViewModel viewModel, Sprite sprite)
        {
            _viewModel = viewModel;
            _spriteRenderer.sprite = sprite;
        }

        private void Update()
        {
            _viewModel.Update();
        }

        public void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}