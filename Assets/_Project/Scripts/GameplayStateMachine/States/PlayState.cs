using System;
using Interfaces;
using UniRx;
using Zenject;

namespace _Project.Scripts.States
{
    public class PlayState : IState, IInitializable, IDisposable
    {
        private readonly CompositeDisposable _disposables = new();
        private readonly IPlayerStateProviderService _playerStateProviderService;

        public PlayState(IPlayerStateProviderService playerStateProviderService)
        {
            _playerStateProviderService = playerStateProviderService;
        }

        public void Enter()
        {
        }

        public void Initialize()
        {
            _playerStateProviderService.PositionProvider
                .Subscribe(OnPlayerStateChanged)
                .AddTo(_disposables);
        }

        public void Exit()
        {
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        private void OnPlayerStateChanged(IPositionProvider playerStateProvider)
        {
            if (playerStateProvider == null)
            {
            }
        }
    }
}
