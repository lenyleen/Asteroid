using System;
using Newtonsoft.Json;

namespace _Project.Scripts.Configs
{
    [Serializable]
    public class SpawnConfig
    {
        public int MaxEnemies { get; private set; }

        public int SpawnLilAsteroidCount { get; private set; }

        [JsonConstructor]
        public SpawnConfig(int maxEnemies, int spawnLilAsteroidCount)
        {
            MaxEnemies = maxEnemies;
            SpawnLilAsteroidCount = spawnLilAsteroidCount;
        }
    }
}
