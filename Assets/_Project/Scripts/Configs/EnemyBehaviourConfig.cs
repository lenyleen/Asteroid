using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "EnemyBehaviourConfig", menuName = "ScriptableObject/EnemyBehaviourConfig")]
    public class EnemyBehaviourConfig : ScriptableObject
    {
        [field: SerializeField] public float Acceleration { get; private set; }
        [field: SerializeField] public float AngularAcceleration { get; private set; }
        [field: SerializeField] public float MaxSpeed { get; private set; }
    }
}
