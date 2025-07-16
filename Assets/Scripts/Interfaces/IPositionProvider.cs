
using UniRx;
using UnityEngine;

namespace Interfaces
{
    public interface IPositionProvider
    {
        public ReactiveProperty<Vector3> Position { get; }
        public ReactiveProperty<float> Rotation { get; }
        
        public ReactiveProperty<Vector2> Velocity { get; }
    }
}