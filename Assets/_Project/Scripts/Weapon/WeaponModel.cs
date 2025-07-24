using Configs;
using UniRx;
using UnityEngine;

namespace Weapon
{
    public class WeaponModel
    {
        private readonly WeaponConfig _weaponConfig;

        private bool _canFire = true;

        public ProjectileType ProjectileType => _weaponConfig.ProjectileType;
        public Vector3 OffsetFromHolder { get; }
        public string Name { get; }
        public ReactiveProperty<float> ReloadTime { get; }
        public ReactiveProperty<int> AmmoCount { get; }

        public WeaponModel(WeaponConfig weaponConfig, string name, Vector3 offsetFromHolder)
        {
            _weaponConfig = weaponConfig;
            Name = name;
            OffsetFromHolder = offsetFromHolder;

            ReloadTime = new ReactiveProperty<float>(weaponConfig.ReloadTimeInSeconds);
            AmmoCount = new ReactiveProperty<int>(_weaponConfig.AmmoCount);
        }

        public bool TryFire()
        {
            if (!_canFire || AmmoCount.Value <= 0)
            {
                return false;
            }

            _canFire = false;

            if (_weaponConfig.Type != WeaponType.Main)
            {
                AmmoCount.Value -= 1;
            }

            ReloadTime.Value = 0;

            return true;
        }

        public void UpdateReloadTime(float deltaTime)
        {
            if (ReloadTime.Value < _weaponConfig.ReloadTimeInSeconds)
            {
                ReloadTime.Value += deltaTime;
                return;
            }

            _canFire = true;
        }
    }
}
