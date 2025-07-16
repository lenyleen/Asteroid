using Factories;
using UnityEngine;
using Zenject;

namespace Services
{
    public class GameService : ITickable
    {
        private PlayerInputController _playerInputController;
        private PlayerSpawner _playerSpawner;
        
        private bool _isStarted = false;

        public GameService(PlayerInputController playerInputController, PlayerSpawner playerSpawner)
        {
            _playerInputController = playerInputController;
            _playerSpawner = playerSpawner;
        }


        public void Tick()
        {
            if (!_isStarted)
            {
                var input = _playerInputController.GetInputValues();
                if (input != Vector2.zero)
                {
                    _isStarted = true;
                    _playerSpawner.SpawnPlayer();
                }
            }
        }
    }
}