using Enemies;
using UnityEngine;

namespace Interfaces
{
    public class KilledEnemyData
    {
        public EnemyType Type { get; }
        public Vector3 Position { get; }
        public int ScoreReward { get; }

        public KilledEnemyData(EnemyType type, Vector3 position, int scoreReward)
        {
            Type = type;
            Position = position;
            ScoreReward = scoreReward;
        }
    }
}
