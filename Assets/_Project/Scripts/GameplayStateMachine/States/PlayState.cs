using System;
using Interfaces;
using Services;
using UniRx;
using Zenject;

namespace _Project.Scripts.States
{
    public class PlayState : IState, IInitializable, IDisposable
    {
        private readonly CompositeDisposable _disposables = new();
        private readonly IPlayerStateProviderService _playerStateProviderService;
        private readonly GameplayStateMachine _gameplayStateMachine;
        private readonly ShipSpawnService _shipSpawnService;
        private readonly EnemySpawnService _enemySpawnService;

        public PlayState(IPlayerStateProviderService playerStateProviderService,
            GameplayStateMachine gameplayStateMachine, ShipSpawnService shipSpawnService)
        {
            _playerStateProviderService = playerStateProviderService;
            _gameplayStateMachine = gameplayStateMachine;
            _shipSpawnService = shipSpawnService;
        }

        public void Enter()
        {
            _shipSpawnService.SpawnShip();
            _enemySpawnService.EnableSpawn(true);
        }

        public void Initialize()
        {
            _playerStateProviderService.PositionProvider
                .Subscribe(OnPlayerStateChanged)
                .AddTo(_disposables);
        }

        public void Exit()
        {
            _enemySpawnService.EnableSpawn(false);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        private void OnPlayerStateChanged(IPositionProvider playerStateProvider)
        {
            if (_playerStateProviderService.PositionProvider.HasValue)
                return;

            _gameplayStateMachine.ChangeState<LoseState>();
        }
    }
}
