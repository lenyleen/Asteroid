using UnityEngine;

namespace _Project.Scripts.Extensions
{
    public static class FloatToSoundConverter
    {
        public static float ConvertToDb(this float input)
        {
            var minLin = 0.0001f;
            var lin = Mathf.Max(input, minLin);
            var dB = 20f * Mathf.Log10(lin);

            return Mathf.Max(dB, -80f);
        }
    }
}
