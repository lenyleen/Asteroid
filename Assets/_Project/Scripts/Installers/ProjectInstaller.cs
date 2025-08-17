using _Project.Scripts.AssetLoaders;
using _Project.Scripts.Factories;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Services;
using _Project.Scripts.Services.AssetProvider;
using Zenject;

namespace _Project.Scripts.Installers
{
    public class ProjectInstaller : MonoInstaller<ProjectInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ProjectAssetProvider>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<ScenesAssetProvider>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<FirebaseInstaller>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<FirebaseRemoteConfigService>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<FirebaseAnalyticsService>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<UnityAdsService>()
                .AsSingle();
        }
    }
}
