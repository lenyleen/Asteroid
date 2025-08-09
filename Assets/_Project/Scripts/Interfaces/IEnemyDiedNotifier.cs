using System;
using _Project.Scripts.Data;

namespace _Project.Scripts.Interfaces
{
    public interface IEnemyDiedNotifier
    {
        public IObservable<KilledEnemyData> OnEnemyKilled { get; }
    }
}
