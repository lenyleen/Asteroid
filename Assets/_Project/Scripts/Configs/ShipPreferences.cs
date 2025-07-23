using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "PlayerPreferences", menuName = "ScriptableObject/PlayerPreferences")]
    public class ShipPreferences : ScriptableObject
    {
        [field: SerializeField] public int Health { get; private set; } = 1;
        [field: SerializeField] public float Acceleration { get; private set; } = 5f;
        [field: SerializeField] public float MaxSpeed { get; private set; } = 10f;
        [field: SerializeField] public float RotationSpeed { get; private set; } = 200f;
        [field: SerializeField] public float Friction { get; private set; } = 0.5f;
        [field: SerializeField] public ColliderConfig ColliderConfig { get; private set; }
    }
}
