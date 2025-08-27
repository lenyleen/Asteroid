using System;
using System.Numerics;
using _Project.Scripts.Data;
using Newtonsoft.Json;

namespace _Project.Scripts.Configs
{
    [Serializable]
    public class ShipPreferences
    {
        public Vector3 StartPosition { get; private set; }

        public int Health { get; private set; }

        public float Acceleration { get; private set; }

        public float MaxSpeed { get; private set; }

        public float RotationSpeed { get; private set; }

        public float Friction { get; private set; }

        public ColliderConfig ColliderConfig { get; private set; }

        public string PlayerShipPrefabAddress { get; private set; }

        public VfxType VFXType { get; private set; }

        [JsonConstructor]
        public ShipPreferences(Vector3 startPosition, int health, float acceleration, float maxSpeed,
            float rotationSpeed, float friction, ColliderConfig colliderConfig, string playerShipPrefabAddress, VfxType vfxType)
        {
            StartPosition = startPosition;
            Health = health;
            Acceleration = acceleration;
            MaxSpeed = maxSpeed;
            RotationSpeed = rotationSpeed;
            Friction = friction;
            ColliderConfig = colliderConfig;
            PlayerShipPrefabAddress = playerShipPrefabAddress;
            VFXType = vfxType;
        }
    }
}
