using System;
using Configs;
using Interfaces;
using Projectiles;
using UniRx;
using UnityEngine;
using Zenject;

namespace Weapon
{
    public class WeaponViewModel : IWeaponInfoProvider
    {
        private readonly WeaponModel _model;
        private readonly ReactiveCommand _onDeath = new();
        private readonly IFactory<ProjectileType, Vector3, IPositionProvider, ProjectileViewModel> _projectileFactory;

        public string Name => _model.Name;
        public IObservable<Unit> OnDeath => _onDeath;
        public ReadOnlyReactiveProperty<float> ReloadTime { get; private set; }
        public ReadOnlyReactiveProperty<int> AmmoCount { get; private set; }

        public WeaponViewModel(
            IFactory<ProjectileType, Vector3, IPositionProvider, ProjectileViewModel> projectileFactory,
            WeaponModel weaponModel)
        {
            _projectileFactory = projectileFactory;
            _model = weaponModel;
        }

        public void Initialize()
        {
            ReloadTime = new ReadOnlyReactiveProperty<float>(_model.ReloadTime);
            AmmoCount = new ReadOnlyReactiveProperty<int>(_model.AmmoCount);
        }

        public void TryFiree(IPositionProvider positionProvider)
        {
            if (!_model.TryFire())
            {
                return;
            }

            var playerRotation = positionProvider.Rotation.Value;
            var rotatedOffset = Quaternion.Euler(0, 0, playerRotation) * _model.OffsetFromHolder;
            var projectileSpawnPos = positionProvider.Position.Value + rotatedOffset;

            _projectileFactory.Create(_model.ProjectileType, projectileSpawnPos, positionProvider);
        }

        public void Update()
        {
            _model.UpdateReloadTime(Time.fixedDeltaTime);
        }

        public void Dispose()
        {
            ReloadTime.Dispose();
            AmmoCount.Dispose();
            _onDeath.Execute();
        }
    }
}
