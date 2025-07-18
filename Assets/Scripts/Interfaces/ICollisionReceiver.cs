using DataObjects;
using UnityEngine.Tilemaps;

namespace Interfaces
{
    public interface ICollisionReceiver
    {
        public ColliderType ColliderType { get; }
        
        public void Collide(ColliderType colliderType, int damage);
    }
}