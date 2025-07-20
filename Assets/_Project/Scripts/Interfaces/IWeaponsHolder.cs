using DataObjects;
using UnityEngine;
using Weapon;

namespace Interfaces
{
    public interface IWeaponsHolder
    {
        public int HeavySlotsCapacity { get; }
        public int MainSlotsCapacity { get; }
        
        public Vector3 ApplyWeapon(WeaponType weaponType, WeaponView weapon);
    }
}