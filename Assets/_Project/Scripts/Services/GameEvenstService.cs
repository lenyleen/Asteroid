using System;
using Interfaces;
using UniRx;

namespace Services
{
    public class GameEvenstService : IGameEvents, IDisposable
    {
        public IObservable<Unit> OnGameStarted => _onGameStarted;
        public IObservable<Unit> OnGameEnded => _onGameEnded;
        public IObservable<int> OnScoreReceived => _onScoreReceived;

        private readonly ReactiveCommand _onGameStarted = new ();
        private readonly ReactiveCommand _onGameEnded = new ();
        private readonly ReactiveCommand<int> _onScoreReceived = new ();
        private readonly CompositeDisposable _disposable = new();
        
        public void ApplyPlayerStateNotifier(IPlayerStateNotifier player)
        {
            player.OnDeath.Take(1)
                .Subscribe(_ => _onGameEnded.Execute())
                .AddTo(_disposable);
        }

        public void RequestGameStart() => _onGameStarted.Execute();


        public void PlayerReceivedScore(int score) => _onScoreReceived.Execute(score);
        
        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}