using _Project.Scripts.Configs;
using _Project.Scripts.Enemies;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Interfaces
{
    public interface IAnalyticsService : IBootstrapInitializable
    {
        public void SendStartGameAnalytics();
        public void SendEndGameAnalytics();
        public void WeaponFire(WeaponType type, string weaponName);
        public void EnemyKilled(EnemyType type);
    }
}
