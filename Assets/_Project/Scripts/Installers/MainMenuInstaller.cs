using _Project.Scripts.Factories;
using _Project.Scripts.MainMenu;
using _Project.Scripts.Other;
using _Project.Scripts.Services;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;
using Zenject;

namespace _Project.Scripts.Installers
{
    public class MainMenuInstaller : MonoInstaller //TODO: плейсхолдер до введения меню
    {
        [SerializeField] private Transform _buttonsParent;
        [SerializeField] private MainMenuStartup _mainMenuStartup;
        [SerializeField] private AssetReference _promoButtonAssetReference;
        [SerializeField] private MainMenuController _mainMenuController;
        [SerializeField] private AudioMixerGroup _sfxGroup;
        [SerializeField] private AudioMixerGroup _musicGroup;

        public override void InstallBindings()
        {
            ProjectContext.Instance.Container.BindInterfacesAndSelfTo<VolumeSettingsProvider>()
                .AsSingle()
                .WithArguments(_sfxGroup, _musicGroup);

            Container.Bind<MainMenuController>()
                .FromInstance(_mainMenuController)
                .AsSingle();

            Container.Bind<MainMenuStartup>()
                .FromInstance(_mainMenuStartup)
                .AsSingle();

            Container.BindInterfacesAndSelfTo<PromoButtonFactory>()
                .AsSingle().WithArguments(_buttonsParent, _promoButtonAssetReference);

            Container.BindInterfacesAndSelfTo<PromoService>()
                .AsSingle();

            Container.Bind<PromoPopUpProvider>()
                .AsSingle();
        }
    }
}
