using UnityEngine;

namespace DataObjects
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObject/WeaponData")]
    public class WeaponData : ScriptableObject
    {
        [field:SerializeField] public WeaponType Type { get; private set; }
        [field: SerializeField] public ProjectileType ProjectileType { get; private set; }
        [field:SerializeField] public Sprite Sprite { get; private set; }
        [field:SerializeField] public int Damage { get; private set; }
        [field:SerializeField] public float ReloadTimeInSeconds { get; private set; }
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