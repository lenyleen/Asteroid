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
        public string Name => _name;
        public ReactiveProperty<float> ReloadTime { get; private set; }
        public ReactiveProperty<int> AmmoCount { get; private set; }
        
        private bool _canFire = true;
        
        private readonly string _name;
        

        public WeaponModel(WeaponData weaponData, string name)
        {
            _weaponData = weaponData;
            _name = name;
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