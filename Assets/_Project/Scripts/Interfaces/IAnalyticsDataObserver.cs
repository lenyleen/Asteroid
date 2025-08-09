using _Project.Scripts.Configs;
using _Project.Scripts.Enemies;

namespace _Project.Scripts.Interfaces
{
    public interface IAnalyticsDataObserver
    {
        public void WeaponFire(WeaponType type, string weaponName);
        public void EnemyKilled(EnemyType type);
    }
}
