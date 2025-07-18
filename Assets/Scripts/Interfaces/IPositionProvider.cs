
using System;
using UniRx;
using UnityEngine;

namespace Interfaces
{
    public interface IPositionProvider
    {
        public IObservable<Unit> OnDeath { get; }
        public ReadOnlyReactiveProperty<Vector3> Position { get; }
        public ReadOnlyReactiveProperty<Vector2> Velocity { get; }
        public ReadOnlyReactiveProperty<float> Rotation { get; } 
    }
}