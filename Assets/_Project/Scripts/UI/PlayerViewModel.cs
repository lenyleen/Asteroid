using System;
using Interfaces;
using UniRx;
using Zenject;

namespace UI
{
    public class PlayerViewModel : IInitializable, IDisposable
    {
        private readonly CompositeDisposable _disposable = new();
        private readonly IGameEvents _gameEvents;

        private readonly PlayerModel _playerModel;

        public PlayerViewModel(PlayerModel model, IGameEvents gameEvents)
        {
            _playerModel = model;
            Score = new ReadOnlyReactiveProperty<int>(_playerModel.Score);
            _gameEvents = gameEvents;
        }

        public ReactiveCommand<bool> OnEndScreenEnable { get; } = new();
        public ReadOnlyReactiveProperty<int> Score { get; }

        public void Dispose()
        {
            _disposable.Dispose();
        }

        public void Initialize()
        {
            _gameEvents.OnGameEnded.Subscribe(_ => OnEndScreenEnable.Execute(true))
                .AddTo(_disposable);

            _gameEvents.OnScoreReceived.Subscribe(score =>
                    _playerModel.UpdateScore(score))
                .AddTo(_disposable);
        }

        public void OnRestartClick(string playerName)
        {
            _playerModel.SavePlayerDataToScore(playerName);
            OnEndScreenEnable.Execute(false);
            _gameEvents.RequestGameStart();
        }
    }
}
