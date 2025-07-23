using System;
using Interfaces;
using UniRx;

namespace Services
{
    public class PlayerDataProviderService : IPlayerPositionProvider, IDisposable
    {
        private IDisposable _disposable;

        public void Dispose()
        {
            _disposable.Dispose();
        }

        public ReactiveProperty<IPositionProvider> PositionProvider { get; } = new();

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
