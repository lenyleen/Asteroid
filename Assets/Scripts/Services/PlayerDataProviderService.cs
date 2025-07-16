using Interfaces;
using UniRx;
using UnityEngine;

namespace Services
{
    public class PlayerDataProviderService : IPlayerPositionProvider
    {
        public IPositionProvider PositionProvider { get; private set; }
        
        public void ApplyPlayer(IPositionProvider player)
        {
            PositionProvider = player;
        }

        public void RemovePlayer(IPositionProvider player)
        {
            PositionProvider = null;
        }
    }
}