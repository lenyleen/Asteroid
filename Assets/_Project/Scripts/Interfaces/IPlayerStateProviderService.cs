using UniRx;

namespace _Project.Scripts.Interfaces
{
    public interface IPlayerStateProviderService
    {
        public ReactiveProperty<IPositionProvider> PositionProvider { get; }
        public void ApplyPlayer(IPositionProvider player);
    }
}
