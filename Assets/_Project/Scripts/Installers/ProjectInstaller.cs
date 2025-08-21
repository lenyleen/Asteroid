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

            Container.BindInterfacesAndSelfTo<PurchaseService>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerInventoryService>()
                .AsSingle();
        }
    }
}
