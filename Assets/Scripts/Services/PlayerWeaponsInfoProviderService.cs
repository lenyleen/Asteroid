using Interfaces;
using UniRx;

namespace Services
{
    public class PlayerWeaponsInfoProviderService : IPlayerWeaponInfoProviderService
    {
        public ReactiveCollection<IWeaponInfoProvider> WeaponInfoProviders { get; private set; }

        public PlayerWeaponsInfoProviderService()
        {
            WeaponInfoProviders = new ReactiveCollection<IWeaponInfoProvider>();
        }
        
        public void RemoveWeaponInfoProvider(IWeaponInfoProvider provider)
        {
            if (WeaponInfoProviders.Contains(provider))
                WeaponInfoProviders.Remove(provider);
        }

        public void ApplyWeaponInfoProvider(IWeaponInfoProvider provider)
        {
            if(WeaponInfoProviders.Contains(provider))
                return;
            
            WeaponInfoProviders.Add(provider);
        }
    }
    
}