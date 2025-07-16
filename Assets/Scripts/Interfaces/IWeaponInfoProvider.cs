using UniRx;

namespace Interfaces
{
    public interface IWeaponInfoProvider
    {
        public string Name { get; }
        public ReadOnlyReactiveProperty<float> ReloadTime { get; }
        public ReadOnlyReactiveProperty<int> AmmoCount { get; }
    }
}