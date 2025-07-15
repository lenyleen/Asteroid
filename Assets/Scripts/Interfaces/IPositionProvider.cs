
using UnityEngine;

namespace Interfaces
{
    public interface IPositionProvider
    {
        public Vector3 Position { get; }
        public Vector2 Direction { get; }
    }
}