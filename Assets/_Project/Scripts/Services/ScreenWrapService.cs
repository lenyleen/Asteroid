using UnityEngine;

namespace _Project.Scripts.Services
{
    public class ScreenWrapService
    {
        private readonly Vector2 _screenMax;
        private readonly Vector2 _screenMin;

        public ScreenWrapService(Camera camera)
        {
            _screenMin = camera.ViewportToWorldPoint(Vector3.zero);
            _screenMax = camera.ViewportToWorldPoint(Vector3.one);
        }

        public Vector3 GetInScreenPosition(Vector3 position)
        {
            var newPosition = Vector3.zero;
            newPosition.x = WrapCoordinate(position.x, _screenMin.x, _screenMax.x);
            newPosition.y = WrapCoordinate(position.y, _screenMin.y, _screenMax.y);

            return position != newPosition ? newPosition : position;
        }

        private static float WrapCoordinate(float value, float min, float max)
        {
            if (value < min)
                return max;

            return value > max ? min : value;
        }
    }
}
