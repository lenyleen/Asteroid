using System;
using Interfaces;
using UniRx;
using Zenject;


namespace UI
{
    public class PlayerViewModel : IInitializable, IDisposable
    {
        public ReactiveCommand<bool> OnEndScreenEnable { get; } = new ();
        public ReadOnlyReactiveProperty<int> Score { get; }
        
        private readonly PlayerModel _playerModel;
        private readonly IGameEvents _gameEvents;
        private readonly CompositeDisposable _disposable = new ();
        
        public PlayerViewModel(PlayerModel model, IGameEvents gameEvents)
        {
           
            _playerModel = model;
            Score = new ReadOnlyReactiveProperty<int>(_playerModel.Score);
            _gameEvents = gameEvents;
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
        
        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}