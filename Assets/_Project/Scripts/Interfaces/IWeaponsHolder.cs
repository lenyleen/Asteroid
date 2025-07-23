using Configs;
using UnityEngine;
using Weapon;

namespace Interfaces
{
    public interface IWeaponsHolder
    {
        public Vector3 ApplyWeapon(WeaponType weaponType, WeaponView weapon);
    }
}
