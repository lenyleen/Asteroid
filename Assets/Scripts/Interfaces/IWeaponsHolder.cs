using DataObjects;
using Weapon;

namespace Interfaces
{
    public interface IWeaponsHolder
    {
        public int HeavySlotsCapacity { get; }
        public int MainSlotsCapacity { get; }
        
        public void ApplyWeapons(WeaponType weaponType, IWeapon weapon);

        public void FireAll();

        public void FireMain();
        public void FireHeavy();
    }
}