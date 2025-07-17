using System;
using Interfaces;
using Signals;
using UniRx;
using UnityEngine;
using Zenject;

namespace Services
{
    public class PlayerDataProviderService : IPlayerPositionProvider, IInitializable, IDisposable
    {
        public ReactiveProperty<IPositionProvider> PositionProvider { get; private set; } =  new ();

        private readonly SignalBus _signalBus;
        private PlayerDataProviderService(SignalBus signalBus)
        {
            _signalBus =  signalBus;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<LoseSignal>(OnLose);
        }

        private void OnLose(LoseSignal signal) => RemovePlayer();
        
        public void ApplyPlayer(IPositionProvider player)
        {
            PositionProvider.Value = player;
        }

        public void RemovePlayer()
        {
            PositionProvider.Value = null;
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<LoseSignal>(OnLose);
        }
    }
}