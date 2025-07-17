using UnityEngine;

namespace Static
{
    public static class RandomPositionGenerator
    {
        public static Vector3 GetRandomPositionOutsideCamera(Camera camera, float offset = 1)
        {
            Camera cam = Camera.main;
            var z = -cam.transform.position.z;

            var bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, z));
            var topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, z));

            var minX = bottomLeft.x;
            var maxX = topRight.x;
            var minY = bottomLeft.y;
            var maxY = topRight.y;

            int side = Random.Range(0, 4);

            return side switch
            {
                0 => new Vector3(minX - offset, Random.Range(minY, maxY), 0),
                
                1 => new Vector3(maxX + offset, Random.Range(minY, maxY), 0),

                2 => new Vector3(Random.Range(minX, maxX), maxY + offset, 0),
                
                3 => new Vector3(Random.Range(minX, maxX), minY - offset, 0),
                
                _ => Vector3.zero
            };
        }

        public static Vector3 GenerateRandomPositionNearPosition(Vector3 centerPosition, float radius = 1f)
        {
            float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            
            float randomDistance = Random.Range(0f, radius);
            
            float offsetX = Mathf.Cos(randomAngle) * randomDistance;
            float offsetY = Mathf.Sin(randomAngle) * randomDistance;
            
            return new Vector3(centerPosition.x + offsetX, centerPosition.y + offsetY, centerPosition.z);
        }
    }
}