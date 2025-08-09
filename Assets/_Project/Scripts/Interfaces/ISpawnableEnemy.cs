using _Project.Scripts.Enemies;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.Interfaces
{
    public interface ISpawnableEnemy
    {
        public EnemyType Type { get; }
        public int Score { get; }
        public ReadOnlyReactiveProperty<Vector3> Position { get; }

        public void Despawn();
    }
}
