using UniRx;

namespace Interfaces
{
    public interface IWeaponInfoProvider
    {
        public ReadOnlyReactiveProperty<float> ReloadTime { get; }
        public ReadOnlyReactiveProperty<int> AmmoCount { get; }
    }
}