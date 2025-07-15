namespace Interfaces
{
    public interface IDamageApplier
    {
        public void TakeCollision(ICollisionReceiver collisionReceiver);
    }
}