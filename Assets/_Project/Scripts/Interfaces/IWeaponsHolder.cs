using _Project.Scripts.Configs;
using _Project.Scripts.Weapon;
using UnityEngine;

namespace _Project.Scripts.Interfaces
{
    public interface IWeaponsHolder
    {
        public Vector3 ApplyWeapon(WeaponType weaponType, WeaponView weapon,Vector3 localPosition);
    }
}
