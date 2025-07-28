using System;

namespace Interfaces
{
    public interface IEnemyDiedNotifier
    {
        public IObservable<KilledEnemyData> OnEnemyKilled { get; }
    }
}
