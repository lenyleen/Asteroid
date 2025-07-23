using System;
using UniRx;

namespace Interfaces
{
    public interface IPlayerStateNotifier
    {
        public IObservable<Unit> OnDeath { get; }
    }
}
