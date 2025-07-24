using System;
using Interfaces;
using UniRx;
using Zenject;

namespace Services
{
    public class PlayerWeaponsInfoProviderService : IPlayerWeaponInfoProviderService, IInitializable, IDisposable
    {
        private readonly CompositeDisposable _disposables = new();
        private readonly CompositeDisposable _infoProvidersDisposables = new();
        private readonly ReactiveCollection<IWeaponInfoProvider> _weaponInfoProviders = new();
        private readonly IGameEvents _gameEvents;

        public IReadOnlyReactiveCollection<IWeaponInfoProvider> WeaponInfoProviders => _weaponInfoProviders;

        public PlayerWeaponsInfoProviderService(IGameEvents gameEvents)
        {
            _gameEvents = gameEvents;
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        public void Initialize()
        {
            _gameEvents.OnGameEnded.Subscribe(_ =>
                    OnLose())
                .AddTo(_disposables);
        }

        public void ApplyWeaponInfoProvider(IWeaponInfoProvider provider)
        {
            if (_weaponInfoProviders.Contains(provider))
            {
                return;
            }

            _weaponInfoProviders.Add(provider);
            provider.OnDeath.Take(1)
                .Subscribe(_ =>
                    RemoveInfoProvider(provider))
                .AddTo(_infoProvidersDisposables);
        }

        private void OnLose()
        {
            _infoProvidersDisposables.Dispose();
            while (WeaponInfoProviders.Count > 0)
            {
                _weaponInfoProviders.Remove(WeaponInfoProviders[^1]);
            }
        }

        private void RemoveInfoProvider(IWeaponInfoProvider provider)
        {
            _weaponInfoProviders.Remove(provider);
        }
    }
}
