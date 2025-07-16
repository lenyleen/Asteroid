using UnityEngine;

namespace Interfaces
{
    public interface IEnemyContext : IBehaviourProvider, IDamageApplier, IDieble
    {
        public void UpdatePosition(Vector3 newPosition);
    }
}