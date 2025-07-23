using System;
using Factories;
using Interfaces;
using UniRx;
using Zenject;

namespace Services
{
    public class ShipSpawnService : IInitializable, IDisposable
    {
        private readonly CompositeDisposable _disposable = new();
        private readonly IGameEvents _gameEvents;
        private readonly PlayerShipFactory _playerShipFactory;

        public ShipSpawnService(PlayerShipFactory playerShipFactory, IGameEvents gameEvents)
        {
            _playerShipFactory = playerShipFactory;
            _gameEvents = gameEvents;
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }

        public void Initialize()
        {
            _gameEvents.OnGameStarted.Subscribe(_ =>
                    SpawnShip())
                .AddTo(_disposable);
        }

        private void SpawnShip()
        {
            var ship = _playerShipFactory.SpawnShip();
            _gameEvents.ApplyPlayerStateNotifier(ship);
        }
    }
}
