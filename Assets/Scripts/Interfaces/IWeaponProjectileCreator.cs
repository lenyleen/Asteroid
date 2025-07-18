using DataObjects;
using Projectiles;
using UnityEngine;

namespace Interfaces
{
    public interface IWeaponProjectileCreator
    {
        public void CreateProjectile(ProjectileType type,IPositionProvider positionProvider);
    }
}