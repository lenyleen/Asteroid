using UnityEngine;
using Weapon;

namespace _Project.Scripts.Weapon
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class WeaponView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private WeaponViewModel _viewModel;

        private void FixedUpdate()
        {
            _viewModel.Update();
        }

        public void Initialize(WeaponViewModel viewModel, Sprite sprite)
        {
            _viewModel = viewModel;
            _spriteRenderer.sprite = sprite;
        }
    }
}
