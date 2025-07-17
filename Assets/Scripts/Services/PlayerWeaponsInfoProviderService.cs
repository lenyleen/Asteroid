using System;
using Interfaces;
using Signals;
using UniRx;
using Zenject;

namespace Services
{
    public class PlayerWeaponsInfoProviderService : IPlayerWeaponInfoProviderService, IInitializable, IDisposable
    {
        public ReactiveCollection<IWeaponInfoProvider> WeaponInfoProviders { get; private set; }
        
        private readonly SignalBus  _signalBus;

        public PlayerWeaponsInfoProviderService(SignalBus signalBus)
        {
            WeaponInfoProviders = new ReactiveCollection<IWeaponInfoProvider>();
            _signalBus = signalBus;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<LoseSignal>(OnLose);
        }

        private void OnLose(LoseSignal loseSignal)
        {
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
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<LoseSignal>(OnLose);
        }
    }
    
}