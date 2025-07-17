using Enemy;
using UnityEngine;

namespace Signals
{
    public class EnemyDestroyedSignal
    {
        public EnemyType Type { get; private set; }
        public Vector3 Position{get; private set;}
        public int Score{get; private set;}

        public EnemyDestroyedSignal(EnemyType type,int score, Vector3 position)
        {
            Type = type;
            Position = position;
            Score = score;
        }
    }
}