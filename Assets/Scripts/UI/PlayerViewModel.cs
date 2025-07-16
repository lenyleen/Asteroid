using System;
using UniRx;
using Zenject;
using IInitializable = Unity.VisualScripting.IInitializable;

namespace UI
{
    public class PlayerViewModel : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly PlayerModel _playerModel;
        
        public ReactiveCommand<bool> OnEndScreenEnable { get; } = new ();
        public ReactiveCommand<bool> OnScoreEnable { get; } = new ();
        public ReadOnlyReactiveProperty<int> Score { get; }

        public PlayerViewModel(PlayerModel model, SignalBus signalBus)
        {
            _signalBus = signalBus;
            _playerModel = model;
            Score = new ReadOnlyReactiveProperty<int>(_playerModel.Score);
        }

        public void Initialize()
        {
            
        }

        public void OnRestartClick(string playerName)
        {
            _playerModel.SavePlayerDataToScore(playerName);
        }
        
        public void Dispose()
        {
            
        }
    }
}