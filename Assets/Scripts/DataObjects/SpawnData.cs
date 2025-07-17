using UnityEngine;

namespace DataObjects
{
    [CreateAssetMenu(fileName = "SpawnData", menuName = "SctiptableObject/Spawn Data")]
    public class SpawnData : ScriptableObject
    {
        [field: SerializeField] public int MaxEnemies { get; private set; }
        [field: SerializeField] public int SpawnLilAsteroidCount { get; private set; }
    }
}