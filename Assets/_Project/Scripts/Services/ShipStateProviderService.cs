using System;
using Interfaces;
using UniRx;

namespace Services
{
    public class ShipStateProviderService : IPlayerStateProviderService, IDisposable
    {
        public ReactiveProperty<IPositionProvider> PositionProvider { get; } = new();

        private IDisposable _disposable = Disposable.Empty;

        public void ApplyPlayer(IPositionProvider player)
        {
            PositionProvider.Value = player;
            _disposable = PositionProvider.Value.OnDeath.Subscribe(_ =>
                RemovePlayer());
        }

        public void Dispose() =>
            _disposable.Dispose();

        private void RemovePlayer()
        {
            _disposable.Dispose();
            PositionProvider.Value = null;
        }
    }
}
