using Factories;
using UI;
using UnityEngine;
using Zenject;

namespace Services
{
    public class GameService : ITickable
    {
        private PlayerInputController _playerInputController;
        private ShipSpawner _shipSpawner;
        private InGameUiViewModel _inGameUi;
        
        private bool _isStarted = false;

        public GameService(PlayerInputController playerInputController, InGameUiViewModel inGameUi, ShipSpawner shipSpawner)
        {
            _inGameUi = inGameUi;
            _playerInputController = playerInputController;
            _shipSpawner = shipSpawner;
        }


        public void Tick()
        {
            if (!_isStarted)
            {
                var input = _playerInputController.GetInputValues();
                if (input != Vector2.zero)
                {
                    _isStarted = true;
                    _shipSpawner.SpawnPlayer();
                    _inGameUi.Start();
                }
            }
        }
    }
}