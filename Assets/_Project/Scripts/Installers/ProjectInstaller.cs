using _Project.Scripts.Services;
using Zenject;

namespace _Project.Scripts.Installers
{
    public class ProjectInstaller : MonoInstaller<ProjectInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<AssetProvider>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<SceneLoader>()
                .AsSingle();
        }
    }
}
