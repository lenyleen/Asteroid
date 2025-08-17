using System;
using Newtonsoft.Json;

namespace _Project.Scripts.Configs
{
    [Serializable]
    public class EnemyBehaviourConfig
    {
        public float Acceleration { get; private set; }

        public float AngularAcceleration { get; private set; }

        public float MaxSpeed { get; private set; }

        [JsonConstructor]
        public EnemyBehaviourConfig(float acceleration, float angularAcceleration, float maxSpeed)
        {
            Acceleration = acceleration;
            AngularAcceleration = angularAcceleration;
            MaxSpeed = maxSpeed;
        }
    }
}
