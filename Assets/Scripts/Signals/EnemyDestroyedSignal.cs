using Enemy;
using UnityEngine;

namespace Signals
{
    public class EnemyDestroyedSignal
    {
        public EnemyType Type;
        public Vector3 Position;

        public EnemyDestroyedSignal(EnemyType type, Vector3 position)
        {
            Type = type;
            Position = position;
        }
    }
}