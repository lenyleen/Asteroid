using UnityEngine;

namespace DataObjects
{
    [CreateAssetMenu(fileName = "PlayerPreferences", menuName = "ScriptableObject/PlayerPreferences")]
    public class PlayerPreferences : ScriptableObject
    {
        [field: SerializeField] public float Acceleration { get; private set; } = 5f;
        [field: SerializeField] public float MaxSpeed { get; private set; } = 10f;
        [field: SerializeField] public float RotationSpeed { get; private set; } = 200f;
        [field: SerializeField] public float MaxRotationSpeed { get; private set; } = 360f;
        [field: SerializeField] public float RotationFriction { get; private set; } = 0.95f;
        [field: SerializeField] public float Friction { get; private set; } = 0.5f;
    }
}