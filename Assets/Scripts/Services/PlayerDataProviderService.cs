using Interfaces;
using UniRx;
using UnityEngine;

namespace Services
{
    public class PlayerDataProviderService : IPlayerPositionProvider
    {
        public ReactiveProperty<IPositionProvider> PositionProvider { get; private set; } =  new ();
        
        public void ApplyPlayer(IPositionProvider player)
        {
            PositionProvider.Value = player;
        }

        public void RemovePlayer(IPositionProvider player)
        {
            PositionProvider.Value = null;
        }
    }
}