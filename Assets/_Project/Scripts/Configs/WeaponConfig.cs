using System;
using Newtonsoft.Json;

namespace _Project.Scripts.Configs
{
    [Serializable]
    public class WeaponConfig
    {
        public WeaponType Type { get; private set; }

        public ProjectileType ProjectileType { get; private set; }

        public string SpriteAddress { get; private set; }

        public float ReloadTimeInSeconds { get; private set; }

        public int AmmoCount { get; private set; }

        [JsonConstructor]
        public WeaponConfig(WeaponType type, ProjectileType projectileType, string spriteAddress,
            float reloadTimeInSeconds, int ammoCount)
        {
            Type = type;
            ProjectileType = projectileType;
            SpriteAddress = spriteAddress;
            ReloadTimeInSeconds = reloadTimeInSeconds;
            AmmoCount = ammoCount;
        }
    }

    public enum WeaponType
    {
        Main = 0,
        Secondary = 1
    }

    public enum ProjectileType
    {
        Laser = 0,
        Bullet = 1
    }
}
