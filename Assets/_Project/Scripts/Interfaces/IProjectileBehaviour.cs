using System;
using UnityEngine;

namespace Interfaces
{
    public interface IProjectileBehaviour : IDisposable
    {
        void Initialize(Vector3 spawnPosition, float shooterRotation);

        void Update(ref Vector3 position, ref float rotation, ref Vector2 velocity);

        public bool CheckDeathAfterCollision();
    }
}
