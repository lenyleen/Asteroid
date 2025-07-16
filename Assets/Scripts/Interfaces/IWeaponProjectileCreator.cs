using DataObjects;
using UnityEngine;

namespace Interfaces
{
    public interface IWeaponProjectileCreator
    {
        public IProjectile CreateProjectile(Vector3 position, float rotation);
    }
}