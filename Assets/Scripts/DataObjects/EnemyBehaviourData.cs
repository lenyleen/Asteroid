using UnityEngine;

namespace DataObjects
{
    [CreateAssetMenu(fileName = "EnemyBehaviourData", menuName = "ScriptableObject/EnemyBehaviourData")]
    public class EnemyBehaviourData : ScriptableObject
    {
        [field: SerializeField] public float acceleration { get; private set; }
        [field: SerializeField] public float angularAcceleration { get; private set; }
    }
}