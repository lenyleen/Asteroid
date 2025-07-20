using UniRx;
using UnityEngine;

namespace Weapon
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class WeaponView : MonoBehaviour 
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        private  WeaponViewModel _viewModel;
        
        public void Initialize(WeaponViewModel viewModel, Sprite sprite)
        {
            _viewModel = viewModel;
            _spriteRenderer.sprite = sprite;
        }

        private void FixedUpdate()
        {
            _viewModel.Update();
        }
    }
}