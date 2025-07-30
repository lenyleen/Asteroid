using System;
using Configs;
using UniRx;
using UnityEngine;

namespace Weapon
{
    public class WeaponModel
    {
        public IObservable<float> ReloadTime => _reloadTime;
        public IObservable<int> AmmoCount => _ammoCount;
        public ProjectileType ProjectileType => _weaponConfig.ProjectileType;
        public float ReloadTimeInSeconds => _weaponConfig.ReloadTimeInSeconds;
        public string Name { get; }
        public Vector3 OffsetFromHolder { get; }

        private readonly ReactiveProperty<float> _reloadTime;
        private readonly ReactiveProperty<int> _ammoCount;
        private readonly WeaponConfig _weaponConfig;

        private bool _canFire = true;

        public WeaponModel(WeaponConfig weaponConfig, string name, Vector3 offsetFromHolder)
        {
            _weaponConfig = weaponConfig;
            Name = name;
            OffsetFromHolder = offsetFromHolder;

            _reloadTime = new ReactiveProperty<float>(weaponConfig.ReloadTimeInSeconds);
            _ammoCount = new ReactiveProperty<int>(_weaponConfig.AmmoCount);
        }

        public bool TryFire()
        {
            if (!_canFire || _ammoCount.Value <= 0)
                return false;

            _canFire = false;

            if (_weaponConfig.Type != WeaponType.Main)
                _ammoCount.Value -= 1;

            _reloadTime.Value = 0;

            return true;
        }

        public void UpdateReloadTime(float deltaTime)
        {
            if (_reloadTime.Value < _weaponConfig.ReloadTimeInSeconds)
            {
                _reloadTime.Value += deltaTime;
                return;
            }

            _canFire = true;
        }
    }
}
