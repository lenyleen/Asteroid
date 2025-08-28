using System;
using _Project.Scripts.Data;
using _Project.Scripts.Enemies;
using Newtonsoft.Json;

namespace _Project.Scripts.Configs
{
    [Serializable]
    public class EnemyConfig
    {
        public string SpriteAddress { get; private set; }

        public int Health { get; private set; }

        public EnemyType Type { get; private set; }

        public int Score { get; private set; }

        public EnemyBehaviourConfig BehaviourConfig { get; private set; }

        public float SpawnTimeInSeconds { get; private set; }

        public ColliderConfig CollisionConfig { get; private set; }

        public VfxType VFXType { get; private set; }

        public string AudioAddress { get; private set; }

        [JsonConstructor]
        public EnemyConfig(string spriteAddress, int health, EnemyType type, int score,
            EnemyBehaviourConfig behaviourConfig, float spawnTimeInSeconds, ColliderConfig collisionConfig, VfxType vfxType, string audioAddress)
        {
            SpriteAddress = spriteAddress;
            Health = health;
            Type = type;
            Score = score;
            BehaviourConfig = behaviourConfig;
            SpawnTimeInSeconds = spawnTimeInSeconds;
            CollisionConfig = collisionConfig;
            VFXType = vfxType;
            AudioAddress = audioAddress;
        }
    }
}
