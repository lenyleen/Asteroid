using Enemy.EnemyBehaviour;
using UniRx;

namespace Interfaces
{
    public interface IBehaviourProvider
    {
        public ReactiveProperty<IEnemyBehaviour> Behaviour { get; }

        public void SetBehaviour(IEnemyBehaviour behaviour);
    }
}