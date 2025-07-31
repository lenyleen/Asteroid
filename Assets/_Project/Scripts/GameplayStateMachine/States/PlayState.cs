using System;
using _Project.Scripts.Factories;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Services;
using _Project.Scripts.UI.ScoreBox;
using UniRx;
using Zenject;

namespace _Project.Scripts.GameplayStateMachine.States
{
    public class PlayState : IState, IInitializable, IDisposable
    {
        public IObservable<Type> OnStateChanged => _changeStateCommand;

        private readonly CompositeDisposable _disposables = new();
        private readonly IPlayerStateProviderService _playerStateProviderService;
        private readonly PlayerShipFactory _shipFactory;
        private readonly EnemySpawnService _enemySpawnService;
        private readonly ReactiveCommand<Type> _changeStateCommand = new();
        private readonly ScoreBoxModel _scoreBoxModel;

        public PlayState(IPlayerStateProviderService playerStateProviderService,PlayerShipFactory shipFactory,
            ScoreBoxModel scoreBoxModel, EnemySpawnService enemySpawnService)
        {
            _playerStateProviderService = playerStateProviderService;
            _shipFactory = shipFactory;
            _scoreBoxModel = scoreBoxModel;
            _enemySpawnService = enemySpawnService;
        }

        public void Enter()
        {
            _scoreBoxModel.Enable(true);
            _shipFactory.SpawnShip();
            _enemySpawnService.EnableSpawn(true);
        }

        public void Initialize()
        {
            _playerStateProviderService.PositionProvider
                .Subscribe(OnPlayerStateChanged)
                .AddTo(_disposables);
        }

        public void Exit() => _enemySpawnService.EnableSpawn(false);

        public void Dispose() => _disposables.Dispose();

        private void OnPlayerStateChanged(IPositionProvider playerStateProvider)
        {
            if (_playerStateProviderService.PositionProvider.Value != null)
                return;

            _changeStateCommand.Execute(typeof(LoseState));
        }
    }
}
