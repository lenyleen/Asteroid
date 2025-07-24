using System;
using Interfaces;
using UniRx;

namespace Services
{
    public class PlayerDataProviderServiceService : IPlayerStateProviderService, IDisposable
    {
        private IDisposable _disposable;

        public ReactiveProperty<IPositionProvider> PositionProvider { get; } = new();

        public void Dispose()
        {
            _disposable.Dispose();
        }

        public void ApplyPlayer(IPositionProvider player)
        {
            PositionProvider.Value = player;
            _disposable = PositionProvider.Value.OnDeath.Subscribe(_ =>
                RemovePlayer());
        }

        private void RemovePlayer()
        {
            _disposable.Dispose();
            PositionProvider.Value = null;
        }
    }
}
