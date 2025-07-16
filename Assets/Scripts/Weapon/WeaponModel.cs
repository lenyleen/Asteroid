using System;
using DataObjects;
using UniRx;
using UnityEngine;

namespace Weapon
{
    public class WeaponModel
    {
        private readonly WeaponData _weaponData;
        public WeaponType Type => _weaponData.Type;
        public ProjectileType ProjectileType => _weaponData.ProjectileType;
        public Sprite Sprite => _weaponData.Sprite;
        public int Damage => _weaponData.Damage;
        public ReactiveProperty<float> ReloadTime { get; private set; }
        public ReactiveProperty<int> AmmoCount { get; private set; }

        private float _reloadTime;
        private float _ammoCount;
        public bool CanFire { get; private set; }
        
        private float _lastFireTime = -999f;

        public WeaponModel(WeaponData weaponData)
        {
            _weaponData = weaponData;
            ReloadTime = new ReactiveProperty<float>(0);
            AmmoCount = new ReactiveProperty<int>(_weaponData.AmmoCount);
        }
        
        public bool TryFire()
        {
            if (!CanFire || AmmoCount.Value <= 0) return false;
            
            _lastFireTime = Time.time;
            CanFire = false;
            AmmoCount.Value -= 1;
            return true;
        }
        
        public void UpdateReloadTime(float deltaTime)
        {
            ReloadTime.Value += deltaTime;
            
            if(ReloadTime.Value < _weaponData.ReloadTimeInSeconds)
                return;
            
            CanFire = true;
            ReloadTime.Value = 0f;
        }
    }
}