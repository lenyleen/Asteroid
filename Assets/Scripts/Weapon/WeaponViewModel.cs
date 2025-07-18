using DataObjects;
using Interfaces;
using Projectiles;
using UniRx;
using UnityEngine;
using Zenject;

namespace Weapon
{
    public class WeaponViewModel : IWeaponInfoProvider
    {
        public WeaponType WeaponType => _model.Type;
        public string Name => _model.Name;
        public ReadOnlyReactiveProperty<float> ReloadTime { get; private set; }
        public ReadOnlyReactiveProperty<int> AmmoCount { get; private set; }

        private readonly WeaponModel _model;

        private IFactory<ProjectileType, IPositionProvider, ProjectileViewModel> _projectileFactory;
        
        public void Initialize()
        {
            ReloadTime = new ReadOnlyReactiveProperty<float>(_model.ReloadTime);
            AmmoCount = new ReadOnlyReactiveProperty<int>(_model.AmmoCount);
        }
        
        public WeaponViewModel( IFactory<ProjectileType,IPositionProvider,ProjectileViewModel> projectileFactory, WeaponModel weaponModel)
        {
            _projectileFactory = projectileFactory;
            _model = weaponModel;
        }
        public void TryFiree(IPositionProvider positionProvider)
        {
            if(!_model.TryFire())
                return;
            
            _projectileFactory.Create(_model.ProjectileType,positionProvider); 
        }
        
        public void Update()
        {
            _model.UpdateReloadTime(Time.fixedDeltaTime);
        }

        public void Dispose()
        {
            ReloadTime.Dispose();
            AmmoCount.Dispose();
        }
        ~WeaponViewModel()
        {
            Debug.Log($"Collected {this.GetType().Name} object");
        }
    }
}