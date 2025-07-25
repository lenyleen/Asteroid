﻿using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "ProjectileConfig", menuName = "ScriptableObject/ProjectileConfig", order = 0)]
    public class ProjectileConfig : ScriptableObject
    {
        [field: SerializeField] public ProjectileType Type { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public ColliderConfig ColliderConfig { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public float LifetimeInSeconds { get; private set; }
    }
}
