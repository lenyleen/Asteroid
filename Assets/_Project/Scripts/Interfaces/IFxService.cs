using _Project.Scripts.Data;
using UnityEngine;

namespace _Project.Scripts.Interfaces
{
    public interface IFxService
    {
        public void PlayVfx(VfxType vfxType,AudioClip audio, Transform parent);
        public void PlayVfx(VfxType vfxType,AudioClip audio, Vector3 position);
    }
}
