namespace Interfaces
{
    public interface IPlayerPositionProvider 
    {
        public IPositionProvider PositionProvider { get; }
        public void ApplyPlayer(IPositionProvider player);
        public void RemovePlayer(IPositionProvider player);
    }
}