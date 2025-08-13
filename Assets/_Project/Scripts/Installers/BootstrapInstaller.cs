using _Project.Scripts.AssetLoaders;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Installers
{
    public class BootstrapInstaller : MonoInstaller<BootstrapInstaller>
    {
        [SerializeField] ProjectAsyncInitializer projectAsyncInitializer;

        public override void InstallBindings()
        {
            Container.Bind<ProjectAsyncInitializer>()
                .FromInstance(projectAsyncInitializer)
                .AsSingle();
        }
    }
}
