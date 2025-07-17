using System;
using Signals;
using UniRx;
using Zenject;


namespace UI
{
    public class PlayerViewModel : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly PlayerModel _playerModel;
        
        public ReactiveCommand<bool> OnEndScreenEnable { get; } = new ();
        public ReadOnlyReactiveProperty<int> Score { get; }

        public PlayerViewModel(PlayerModel model, SignalBus signalBus)
        {
            _signalBus = signalBus;
            _playerModel = model;
            Score = new ReadOnlyReactiveProperty<int>(_playerModel.Score);
        }

        public void Initialize()
        {
            _signalBus.Subscribe<LoseSignal>(OnLose);
            _signalBus.Subscribe<EnemyDestroyedSignal>(OnEnemyDestroyed);
        }

        public void OnRestartClick(string playerName)
        {
            _playerModel.SavePlayerDataToScore(playerName);
            OnEndScreenEnable.Execute(false);
            _signalBus.Fire<GameStarted>();
        }

        private void OnLose(LoseSignal loseSignal)
        {
            OnEndScreenEnable.Execute(true);
        }
        
        private void OnEnemyDestroyed(EnemyDestroyedSignal signal)
        {
            _playerModel.UpdateScore(signal.Score);
        }
        
        public void Dispose()
        {
            _signalBus.TryUnsubscribe<EnemyDestroyedSignal>(OnEnemyDestroyed);
        }
    }
}