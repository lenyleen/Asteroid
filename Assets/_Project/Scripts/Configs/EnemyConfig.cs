﻿using Enemies;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "ScriptableObject/EnemyConfig")]
    public class EnemyConfig : ScriptableObject
    {
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public int Health { get; private set; }
        [field: SerializeField] public EnemyType Type { get; private set; }
        [field: SerializeField] public int Score { get; private set; }
        [field: SerializeField] public EnemyBehaviourConfig BehaviourConfig { get; private set; }
        [field: SerializeField] public float SpawnTimeInSeconds { get; private set; }
        [field: SerializeField] public ColliderConfig CollisionConfig { get; private set; }
    }
}
