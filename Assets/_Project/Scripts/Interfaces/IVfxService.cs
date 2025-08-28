using _Project.Scripts.Data;
using UnityEngine;

namespace _Project.Scripts.Interfaces
{
    public interface IVfxService
    {
        public void PlayVfx(VfxType vfxType, Transform parent);
        public void PlayVfx(VfxType vfxType, Vector3 position);
    }
}
