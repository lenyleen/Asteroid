using UniRx;

namespace Interfaces
{
    public interface IPlayerWeaponInfoProviderService
    {
        public IReadOnlyReactiveCollection<IWeaponInfoProvider> WeaponInfoProviders { get; }

        public void ApplyWeaponInfoProvider(IWeaponInfoProvider provider);
    }
}
