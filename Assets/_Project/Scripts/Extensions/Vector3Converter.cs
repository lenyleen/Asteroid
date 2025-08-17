using UnityEngine;

namespace _Project.Scripts.Extensions
{
    public static class Vector3Converter
    {
        public static Vector3 ToUnityVector3(this System.Numerics.Vector3 val)
        {
            return new Vector3(val.X, val.Y, val.Z);
        }
    }
}
