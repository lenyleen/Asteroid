using Enemy;
using UnityEngine;

namespace DataObjects
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObject/EnemyData")]
    public class EnemyData : ScriptableObject
    {
        [field:SerializeField] public Sprite Sprite { get; private set; }
        [field:SerializeField] public int Health { get; private set; }
        [field:SerializeField] public EnemyType Type { get; private set; }
        [field: SerializeField] public EnemyBehaviourData  BehaviourData { get; private set; }
        [field: SerializeField] public float SpawnTimeInSeconds { get; private set; }
        [field: SerializeField] public int MaxCount { get; private set; }
    }
}