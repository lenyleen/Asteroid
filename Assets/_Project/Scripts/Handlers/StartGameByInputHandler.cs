using Interfaces;
using UnityEngine;
using Zenject;

namespace Handlers
{
    public class StartGameByInputHandler : ITickable
    {
        private readonly IGameEvents _gameEvents;
        private readonly PlayerInputController _inputController;

        private bool _isStarted;

        public StartGameByInputHandler(IGameEvents gameEvents, PlayerInputController inputController)
        {
            _gameEvents = gameEvents;
            _inputController = inputController;
        }

        public void Tick()
        {
            if (_isStarted)
            {
                return;
            }

            if (_inputController.GetInputValues() == Vector2.zero)
            {
                return;
            }

            _isStarted = true;
            _gameEvents.RequestGameStart();
        }
    }
}
