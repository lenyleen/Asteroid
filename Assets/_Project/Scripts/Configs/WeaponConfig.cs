using UnityEngine;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(fileName = "WeaponConfig", menuName = "ScriptableObject/WeaponConfig")]
    public class WeaponConfig : ScriptableObject
    {
        [field: SerializeField] public WeaponType Type { get; private set; }
        [field: SerializeField] public ProjectileType ProjectileType { get; private set; }
        [field: SerializeField] public string SpriteAddress { get; private set; }
        [field: SerializeField] public float ReloadTimeInSeconds { get; private set; }
        [field: SerializeField] public int AmmoCount { get; private set; }
    }

    public enum WeaponType
    {
        Main,
        Secondary
    }

    public enum ProjectileType
    {
        Laser,
        Bullet
    }
}
