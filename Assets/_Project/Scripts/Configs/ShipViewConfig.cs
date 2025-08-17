using System;
using System.Collections.Generic;
using System.Numerics;
using Newtonsoft.Json;

namespace _Project.Scripts.Configs
{
    [Serializable]
    public class ShipViewConfig
    {
        public string ShipSpriteAdress { get; private set; }

        public List<Vector3> HeavyWeaponSlots { get; private set; }

        public List<Vector3> LightWeaponSlots { get; private set; }

        public WeaponType MainWeaponType { get; private set; }

        public WeaponType HeavyWeaponType { get; private set; }

        [JsonConstructor]
        public ShipViewConfig(string shipSpriteAdress, List<Vector3> heavyWeaponSlots, List<Vector3> lightWeaponSlots,
            WeaponType mainWeaponType, WeaponType heavyWeaponType)
        {
            ShipSpriteAdress = shipSpriteAdress;
            HeavyWeaponSlots = heavyWeaponSlots;
            LightWeaponSlots = lightWeaponSlots;
            MainWeaponType = mainWeaponType;
            HeavyWeaponType = heavyWeaponType;
        }
    }
}
