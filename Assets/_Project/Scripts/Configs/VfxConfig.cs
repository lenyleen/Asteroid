using System;
using _Project.Scripts.Data;

namespace _Project.Scripts.Configs
{
    [Serializable]
    public class VfxConfig
    {
        public VfxType  VfxType { get; private set; }
        public string ParticlePrefabAddress { get; private set; }
    }
}
