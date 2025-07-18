﻿using Interfaces;
using UnityEngine;
using Zenject;

namespace Services
{
    public class ScreenWrapService
    {
        private readonly Camera _camera;
        private readonly Vector2 _screenMin;
        private readonly Vector2 _screenMax;

        public ScreenWrapService(Camera camera)
        {
            _camera = camera;
            
            _screenMin = _camera.ViewportToWorldPoint(Vector3.zero);
            _screenMax = _camera.ViewportToWorldPoint(Vector3.one);
        }
        
        public Vector3 GetInScreenPosition(Vector3 position)
        {
            var newPosition = Vector3.zero;
            newPosition.x = WrapCoordinate(position.x, _screenMin.x, _screenMax.x);
            newPosition.y = WrapCoordinate(position.y, _screenMin.y, _screenMax.y);
            
            return position != newPosition ? newPosition : position;
        }

        private float WrapCoordinate(float value, float min, float max)
        {
            if(value < min)
                return max;
            
            return value > max ? min : value;
        }
    }
}