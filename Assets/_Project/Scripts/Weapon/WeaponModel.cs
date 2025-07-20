using DataObjects;
using UniRx;
using UnityEngine;

namespace Weapon
{
    public class WeaponModel
    {
        public ProjectileType ProjectileType => _weaponData.ProjectileType;
        public Vector3 OffsetFromHolder { get; }
        public string Name { get; }
        public ReactiveProperty<float> ReloadTime { get;}
        public ReactiveProperty<int> AmmoCount { get; }
        
        private readonly WeaponData _weaponData;

        private bool _canFire = true;
        
        public WeaponModel(WeaponData weaponData, string name, Vector3 offsetFromHolder)
        {
            _weaponData = weaponData;
            Name = name;
            OffsetFromHolder = offsetFromHolder;
            
            ReloadTime = new ReactiveProperty<float>(weaponData.ReloadTimeInSeconds);
            AmmoCount = new ReactiveProperty<int>(_weaponData.AmmoCount);
        }
        
        public bool TryFire()
        {
            if (!_canFire || AmmoCount.Value <= 0) return false;
            
            _canFire = false;
            
            if(_weaponData.Type != WeaponType.Main)
                AmmoCount.Value -= 1;

            ReloadTime.Value = 0;
            
            return true;
        }
        
        public void UpdateReloadTime(float deltaTime)
        {
            if (ReloadTime.Value < _weaponData.ReloadTimeInSeconds)
            {
                ReloadTime.Value += deltaTime;
                return;
            }

            _canFire = true;
        }
    }
}