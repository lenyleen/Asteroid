using UnityEngine;

namespace DataObjects
{
    [CreateAssetMenu(fileName = "ProjectileData", menuName = "ScriptableObject/ProjectileData", order = 0)]
    public class ProjectileData : ScriptableObject
    {
        [field: SerializeField] public ProjectileType Type { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public ColliderData ColliderData { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public float LifetimeInSeconds { get; private set; }
    }
}