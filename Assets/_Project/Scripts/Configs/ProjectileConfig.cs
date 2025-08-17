using System;
using Newtonsoft.Json;
using UnityEngine;

namespace _Project.Scripts.Configs
{
    [Serializable]
    public class ProjectileConfig
    {
        public ProjectileType Type { get; private set; }

        public string SpriteAddress { get; private set; }

        public ColliderConfig ColliderConfig { get; private set; }

        public float Speed { get; private set; }

        public float LifetimeInSeconds { get; private set; }

        [JsonConstructor]
        public ProjectileConfig(ProjectileType type, string spriteAddress, ColliderConfig colliderConfig,
            float speed, float lifetimeInSeconds)
        {
            Type = type;
            SpriteAddress = spriteAddress;
            ColliderConfig = colliderConfig;
            Speed = speed;
            LifetimeInSeconds = lifetimeInSeconds;
        }
    }
}
