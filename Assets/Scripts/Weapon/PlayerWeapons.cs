using System.Collections.Generic;
using DataObjects;
using Interfaces;
using UnityEngine;

namespace Weapon
{
    public class PlayerWeapons : MonoBehaviour, IWeaponsHolder
    {
        public int HeavySlotsCapacity => _heavySlots.Capacity;
        public int MainSlotsCapacity => _mainSlots.Capacity;
        
        [SerializeField] private List<Transform> _heavySlots;
        [SerializeField] private List<Transform> _mainSlots;
        
        private List<IWeapon> _mainWeapons;
        private List<IWeapon> _heavyWeapons;

        public void ApplyWeapons(WeaponType weaponType, IWeapon weapon)
        {
            
        }

        public void FireAll()
        {
            FireMain();
            FireHeavy();
        }

        public void FireMain()
        {
            _mainWeapons.ForEach(slot => slot.Fire());
        }

        public void FireHeavy()
        {
            _heavyWeapons.ForEach(slot => slot.Fire());
        }
    }
}