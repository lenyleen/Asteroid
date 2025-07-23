using Enemies;
using UniRx;
using UnityEngine;

namespace Interfaces
{
    public interface ISpawnableEnemy
    {
        public EnemyType Type { get; }
        public int Score { get; }
        public ReadOnlyReactiveProperty<Vector3> Position { get; }

        public void Despawn();
    }
}
