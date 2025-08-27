using _Project.Scripts.AssetLoaders;
using _Project.Scripts.Configs;
using _Project.Scripts.Factories;
using _Project.Scripts.Services;
using _Project.Scripts.UI;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Installers
{
    public class BootstrapInstaller : MonoInstaller<BootstrapInstaller>
    {
        [SerializeField] ProjectAsyncInitializer projectAsyncInitializer;
        [SerializeField] CrashHandler _crashHandler;
        [SerializeField] PopUpsConfig _popUpsConfig;

        public override void InstallBindings()
        {
            var projectContainer = ProjectContext.Instance.Container;

            DontDestroyOnLoad(_crashHandler);

            Container.Bind<ProjectAsyncInitializer>()
                .FromInstance(projectAsyncInitializer)
                .AsSingle();

            Container.Bind<PlayerProgressSaveCheckHandler>()
                .AsSingle();

            projectContainer.BindInterfacesAndSelfTo<PopUpFactory>()
                .AsSingle()
                .WithArguments(_popUpsConfig, _crashHandler.transform);

            projectContainer.Bind<UiService>()
                .AsSingle();

            projectContainer.BindInterfacesAndSelfTo<CrashHandler>()
                .FromInstance(_crashHandler)
                .AsSingle();
        }
    }
}
