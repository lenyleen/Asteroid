using UniRx;

namespace Interfaces
{
    public interface IPlayerWeaponInfoProviderService
    {
        public ReactiveCollection<IWeaponInfoProvider> WeaponInfoProviders { get; }

        public void ApplyWeaponInfoProvider(IWeaponInfoProvider provider);
    }
}