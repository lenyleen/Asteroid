using UniRx;

namespace Interfaces
{
    public interface IPlayerStateProviderService
    {
        public ReactiveProperty<IPositionProvider> PositionProvider { get; }
        public void ApplyPlayer(IPositionProvider player);
    }
}
