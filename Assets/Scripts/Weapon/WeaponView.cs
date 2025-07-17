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
            
            _viewModel.OnProjectileCreated.Subscribe(ApllyProjectile).AddTo(_disposables);
            
            _spriteRenderer.sprite = sprite;
        }
        
        private void ApllyProjectile(IProjectile projectile)
        {
            if(projectile is not Projectile projectileInGame)
                return;

            var projectileTransform = projectileInGame.transform;
            
            projectileTransform.SetParent(this.transform);
            projectileTransform.localPosition = Vector3.zero;
        }

        public void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}