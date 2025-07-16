using UnityEngine;

namespace Interfaces
{
    public interface IProjectile
    {
        public void ApplyBehaviour(IProjectileBehaviour projectileBehaviour);
    }
}