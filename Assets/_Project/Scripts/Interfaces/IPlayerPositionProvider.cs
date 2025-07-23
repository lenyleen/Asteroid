using UniRx;

namespace Interfaces
{
    public interface IPlayerPositionProvider
    {
        public ReactiveProperty<IPositionProvider> PositionProvider { get; }
        public void ApplyPlayer(IPositionProvider player);
    }
}
