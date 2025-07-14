using Interfaces;
using UnityEngine;
using Zenject;

namespace Services
{
    public class ScreenWrapService : IFixedTickable
    {
        private readonly IPositionMutator  _positionMutator;
        private readonly Camera _camera;
        private readonly Vector2 _screenMin;
        private readonly Vector2 _screenMax;

        public ScreenWrapService(IPositionMutator positionMutator, Camera camera)
        {
            _positionMutator = positionMutator;
            _camera = camera;
            
            _screenMin = _camera.ViewportToWorldPoint(Vector3.zero);
            _screenMax = _camera.ViewportToWorldPoint(Vector3.one);
        }
        
        public void FixedTick()
        {
            var newPosition = _positionMutator.Position;
            
            newPosition.x = WrapCoordinate(newPosition.x, _screenMin.x, _screenMax.x);
            newPosition.y = WrapCoordinate(newPosition.y, _screenMin.y, _screenMax.y);
            
            if(_positionMutator.Position != newPosition)
                _positionMutator.SetPosition(newPosition);
        }

        private float WrapCoordinate(float value, float min, float max)
        {
            if(value < min)
                return max;
            
            return value > max ? min : value;
        }
    }
}