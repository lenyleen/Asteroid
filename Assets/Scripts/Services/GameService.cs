using System;
using Factories;
using Signals;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Services
{
    public class GameService : ITickable, IInitializable, IDisposable
    {
        private PlayerInputController _playerInputController;
        private ShipSpawner _shipSpawner;
        private InGameUiViewModel _inGameUi;
        private SignalBus _signalBus;
        
        private bool _isStarted = false;

        public GameService(PlayerInputController playerInputController, InGameUiViewModel inGameUi, ShipSpawner shipSpawner, SignalBus signalBus)
        {
            _inGameUi = inGameUi;
            _playerInputController = playerInputController;
            _shipSpawner = shipSpawner;
            _signalBus = signalBus;
        }
        
        public void Initialize()
        {
            _signalBus.Subscribe<GameStarted>(RestartGame);
        }

        private void RestartGame(GameStarted signal)
        {
            _shipSpawner.SpawnPlayer();
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

        

        public void Dispose()
        {
            _signalBus.Unsubscribe<GameStarted>(RestartGame);
        }
    }
}