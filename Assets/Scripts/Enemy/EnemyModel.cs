using UniRx;

namespace Enemy
{
    public class EnemyModel
    {
        public ReactiveProperty<int> Health;
        public ReactiveCommand OnDeath;

        public EnemyModel(int health)
        {
            Health = new ReactiveProperty<int>(health) ;
            OnDeath = new ReactiveCommand();
        }
        public void TakeHit(int damage)
        {
            Health.Value -= damage;

            if (Health.Value <= 0)
                OnDeath.Execute();
        }
    }
}