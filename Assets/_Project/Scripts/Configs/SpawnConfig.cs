using UnityEngine;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(fileName = "SpawnConfig", menuName = "ScriptableObject/SpawnConfig")]
    public class SpawnConfig : ScriptableObject
    {
        [field: SerializeField] public int MaxEnemies { get; private set; }
        [field: SerializeField] public int SpawnLilAsteroidCount { get; private set; }
    }
}
