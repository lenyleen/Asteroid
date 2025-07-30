using System;
using Interfaces;
using UniRx;
using Zenject;

namespace Services
{
    public class PlayerWeaponsInfoProviderService : IPlayerWeaponInfoProviderService, IDisposable
    {
        public IReadOnlyReactiveCollection<IWeaponInfoProvider> WeaponInfoProviders => _weaponInfoProviders;

        private readonly CompositeDisposable _disposables = new();
        private readonly CompositeDisposable _infoProvidersDisposables = new();
        private readonly ReactiveCollection<IWeaponInfoProvider> _weaponInfoProviders = new();

        public void ApplyWeaponInfoProvider(IWeaponInfoProvider provider)
        {
            if (_weaponInfoProviders.Contains(provider))
                return;

            _weaponInfoProviders.Add(provider);
            provider.OnDeath.Take(1)
                .Subscribe(_ =>
                    RemoveInfoProvider(provider))
                .AddTo(_infoProvidersDisposables);
        }

        public void Dispose() =>
            _disposables.Dispose();

        private void OnLose()
        {
            _infoProvidersDisposables.Dispose();
            while (WeaponInfoProviders.Count > 0)
                _weaponInfoProviders.Remove(WeaponInfoProviders[^1]);
        }

        private void RemoveInfoProvider(IWeaponInfoProvider provider) =>
            _weaponInfoProviders.Remove(provider);
    }
}
