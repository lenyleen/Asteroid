using UnityEngine;

namespace Interfaces
{
    public interface IProjectile
    {
        public void ApplyParent(Transform transform);
        public void ApplyBehaviour(IProjectileBehaviour projectileBehaviour);
    }
}