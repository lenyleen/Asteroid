using System;
using UniRx;

namespace Interfaces
{
    public interface IWeaponInfoProvider
    {
        public IObservable<Unit> OnDeath { get; }
        public string Name { get; }
        public ReadOnlyReactiveProperty<float> ReloadTimePercent { get; }
        public ReadOnlyReactiveProperty<int> AmmoCount { get; }
    }
}
