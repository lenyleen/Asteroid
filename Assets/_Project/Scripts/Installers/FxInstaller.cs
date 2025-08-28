using _Project.Scripts.Factories;
using _Project.Scripts.Pools;
using _Project.Scripts.Services;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Installers
{
    public class FxInstaller : MonoInstaller<FxInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ParticleFactory>()
                .AsSingle();

            Container.Bind<ParticlePool>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<VfxService>()
                .AsSingle();
        }
    }
}
