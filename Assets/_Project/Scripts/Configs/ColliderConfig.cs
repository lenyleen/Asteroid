using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace _Project.Scripts.Configs
{
    [Serializable]
    public class ColliderConfig
    {
        public ColliderType ColliderType { get; private set; }

        public int Damage { get; private set; }

        public List<ColliderType> AcceptableColliderTypes { get; private set; }

        [JsonConstructor]
        public ColliderConfig(ColliderType colliderType, int damage, List<ColliderType> acceptableColliderTypes)
        {
            ColliderType = colliderType;
            Damage = damage;
            AcceptableColliderTypes = acceptableColliderTypes;
        }
    }
}
