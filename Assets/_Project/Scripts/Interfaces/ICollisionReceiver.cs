using _Project.Scripts.Configs;

namespace _Project.Scripts.Interfaces
{
    public interface ICollisionReceiver
    {
        public ColliderType ColliderType { get; }

        public void Collide(ColliderType colliderType, int damage);
    }
}
