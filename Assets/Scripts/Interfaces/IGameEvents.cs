using System;
using UniRx;

namespace Interfaces
{
    public interface IGameEvents
    {
        public IObservable<Unit> OnGameStarted{get;}
        public IObservable<Unit> OnGameEnded{get;}
        public IObservable<int> OnScoreReceived { get; }
        public void RequestGameStart();

        public void ApplyPlayerStateNotifier(IPlayerStateNotifier player);
        public void PlayerReceivedScore(int score);
    }
}