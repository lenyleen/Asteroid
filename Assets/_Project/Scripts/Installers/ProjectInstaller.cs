using _Project.Scripts.Factories;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Services;
using _Project.Scripts.Services.AssetProvider;
using Unity.Services.Economy;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace _Project.Scripts.Installers
{
    public class ProjectInstaller : MonoInstaller<ProjectInstaller>
    {
        [SerializeField] private AudioMixerGroup _musicMixerGroup;
        [SerializeField] private AudioMixerGroup _sfxMixerGroup;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ProjectAssetProvider>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<ScenesAssetProvider>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<UnityServicesInstaller>()
                .AsSingle();

            Container.Bind<BgmHandlerFactory>()
                .AsSingle()
                .WithArguments(_musicMixerGroup);

            Container.BindInterfacesAndSelfTo<FirebaseInstaller>()
                .AsSingle();

            Container.Bind<LocalSaveLoadService>()
                .AsSingle();

            Container.Bind<RemoteSaveLoadService>()
                .AsSingle();

            Container.Bind<IProjectImportanceInitializable>()
                .To<RemoteSaveLoadService>()
                .FromResolve();

            Container.BindInterfacesAndSelfTo<FirebaseRemoteConfigService>()
                .AsSingle();

            Container.Bind<PlayerProgressSaveCheckHandler>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<SaveServiceProvider>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<VolumeSettingsProvider>()
                .AsSingle()
                .WithArguments(_sfxMixerGroup, _musicMixerGroup);

            Container.BindInterfacesAndSelfTo<FirebaseAnalyticsService>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<UnityAdsService>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<StoreControllerInstaller>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<PurchaseService>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerInventoryService>()
                .AsSingle();
        }
    }
}
