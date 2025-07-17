using DataObjects;
using Interfaces;
using UniRx;
using UnityEngine;
using Zenject;

namespace Weapon
{
    public class WeaponViewModel : IFixedTickable, IWeaponInfoProvider
    {
        public WeaponType WeaponType => _model.Type;
        
        public ReactiveCommand<IProjectile> OnProjectileCreated { get; } = new();
        public string Name => _model.Name;
        public ReadOnlyReactiveProperty<float> ReloadTime { get; private set; }
        public ReadOnlyReactiveProperty<int> AmmoCount { get; private set; }

        private readonly WeaponModel _model;
       
        private readonly IWeaponProjectileCreator _projectileCreator;
        
        public void Initialize()
        {
            ReloadTime = new ReadOnlyReactiveProperty<float>(_model.ReloadTime);
            AmmoCount = new ReadOnlyReactiveProperty<int>(_model.AmmoCount);
        }
        
        public WeaponViewModel(IWeaponProjectileCreator projectileCreator, WeaponModel weaponModel)
        {
            _projectileCreator = projectileCreator;
            _model = weaponModel;
        }
        public void TryFiree(Vector3 position, float rotation)
        {
            if(!_model.TryFire())
                return;
            
            var projectile = _projectileCreator.CreateProjectile(position,rotation);
            OnProjectileCreated.Execute(projectile);
        }
        
        public void FixedTick()
        {
            _model.UpdateReloadTime(Time.fixedDeltaTime);
        }

        public void Dispose()
        {
            ReloadTime.Dispose();
            AmmoCount.Dispose();
        }
    }
}