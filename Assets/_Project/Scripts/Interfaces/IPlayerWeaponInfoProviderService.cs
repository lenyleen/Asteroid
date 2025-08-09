using UniRx;

namespace _Project.Scripts.Interfaces
{
    public interface IPlayerWeaponInfoProviderService
    {
        public IReadOnlyReactiveCollection<IWeaponInfoProvider> WeaponInfoProviders { get; }

        public void ApplyWeaponInfoProvider(IWeaponInfoProvider provider);
    }
}
