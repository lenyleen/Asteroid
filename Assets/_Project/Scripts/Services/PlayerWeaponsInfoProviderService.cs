using System;
using Interfaces;
using UniRx;
using Zenject;

namespace Services
{
    public class PlayerWeaponsInfoProviderService : IPlayerWeaponInfoProviderService, IInitializable, IDisposable
    {
        public ReactiveCollection<IWeaponInfoProvider> WeaponInfoProviders { get;}
        
        private readonly IGameEvents _gameEvents;
        private readonly CompositeDisposable _disposables = new ();
        
        private CompositeDisposable _infoProvidersDisposables = new ();

        public PlayerWeaponsInfoProviderService(IGameEvents gameEvents)
        {
            WeaponInfoProviders = new ReactiveCollection<IWeaponInfoProvider>();
            
            _gameEvents = gameEvents;
        }

        public void Initialize()
        {
            _gameEvents.OnGameEnded.Subscribe(_ =>
                OnLose())
                .AddTo(_disposables);
        }

        private void OnLose()
        {
            _infoProvidersDisposables.Dispose();
            while (WeaponInfoProviders.Count > 0)
            {
                WeaponInfoProviders.Remove(WeaponInfoProviders[^1]);
            }
        }

        public void ApplyWeaponInfoProvider(IWeaponInfoProvider provider)
        {
            if(WeaponInfoProviders.Contains(provider))
                return;
            
            WeaponInfoProviders.Add(provider);
            provider.OnDeath.Take(1)
                .Subscribe(_ =>
                RemoveInfoProvider(provider))
                .AddTo(_infoProvidersDisposables);
        }

        private void RemoveInfoProvider(IWeaponInfoProvider provider)
        {
            WeaponInfoProviders.Remove(provider);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
    
}