using System;
using Interfaces;
using UniRx;

namespace Services
{
    public class GameEvenstService : IGameEvents, IDisposable
    {
        private readonly CompositeDisposable _disposable = new();
        private readonly ReactiveCommand _onGameEnded = new();

        private readonly ReactiveCommand _onGameStarted = new();
        private readonly ReactiveCommand<int> _onScoreReceived = new();

        public IObservable<Unit> OnGameStarted => _onGameStarted;
        public IObservable<Unit> OnGameEnded => _onGameEnded;
        public IObservable<int> OnScoreReceived => _onScoreReceived;

        public void Dispose()
        {
            _disposable.Dispose();
        }

        public void ApplyPlayerStateNotifier(IPlayerStateNotifier player)
        {
            player.OnDeath.Take(1)
                .Subscribe(_ => _onGameEnded.Execute())
                .AddTo(_disposable);
        }

        public void RequestGameStart()
        {
            _onGameStarted.Execute();
        }

        public void PlayerReceivedScore(int score)
        {
            _onScoreReceived.Execute(score);
        }
    }
}
