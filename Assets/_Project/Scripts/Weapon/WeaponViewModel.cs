using System;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Weapon;
using Cysharp.Threading.Tasks;
using Factories;
using UniRx;
using UnityEngine;

namespace Weapon
{
    public class WeaponViewModel : IWeaponInfoProvider
    {
        public string Name => _model.Name;
        public IObservable<Unit> OnDeath => _onDeath;
        public ReadOnlyReactiveProperty<float> ReloadTimePercent { get; }
        public ReadOnlyReactiveProperty<int> AmmoCount { get; private set; }

        private readonly WeaponModel _model;
        private readonly ReactiveCommand _onDeath = new();
        private readonly ReactiveProperty<float> _reloadTimePercent = new();
        private readonly ProjectileFactory _projectileFactory;
        private readonly IAnalyticsDataObserver _analyticsDataObserver;

        public WeaponViewModel(ProjectileFactory projectileFactory, WeaponModel weaponModel,
            IAnalyticsService analyticsDataObserver)
        {

            ReloadTimePercent = new ReadOnlyReactiveProperty<float>(_reloadTimePercent);
            _projectileFactory = projectileFactory;
            _model = weaponModel;
            _analyticsDataObserver = analyticsDataObserver;
        }

        public void Initialize()
        {
            _model.ReloadTime.Subscribe(time =>
                _reloadTimePercent.Value = time / _model.ReloadTimeInSeconds);

            AmmoCount = new ReadOnlyReactiveProperty<int>(_model.AmmoCount);
        }

        public async UniTask TryFiree(IPositionProvider positionProvider)
        {
            if (!_model.TryFire())
                return;

            var playerRotation = positionProvider.Rotation.Value;
            var rotatedOffset = Quaternion.Euler(0, 0, playerRotation) * _model.OffsetFromHolder;
            var projectileSpawnPos = positionProvider.Position.Value + rotatedOffset;

            _projectileFactory.Create(_model.ProjectileType, projectileSpawnPos, positionProvider);

            _analyticsDataObserver.WeaponFire(_model.Type, _model.Name);
            await _projectileFactory.Create(_model.ProjectileType, projectileSpawnPos, positionProvider);
        }

        public void Update()
        {
            _model.UpdateReloadTime(Time.fixedDeltaTime);
        }

        public void Dispose()
        {
            ReloadTimePercent.Dispose();
            AmmoCount.Dispose();
            _onDeath.Execute();
        }
    }
}
