using UniRx;

namespace Interfaces
{
    public interface IPlayerWeaponInfoProviderService
    {
        public ReactiveCollection<IWeaponInfoProvider> WeaponInfoProviders { get; }

        public void RemoveWeaponInfoProvider(IWeaponInfoProvider provider);

        public void ApplyWeaponInfoProvider(IWeaponInfoProvider provider);
    }
}