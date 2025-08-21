using _Project.Scripts.Factories;
using _Project.Scripts.MainMenu;
using _Project.Scripts.Other;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.Installers
{
    public class MainMenuInstaller : MonoInstaller //TODO: плейсхолдер до введения меню
    {
        [SerializeField] private MainMenuPlayButtonTemp _temp;
        [SerializeField] private Transform _buttonsParent;
        [SerializeField] private MainMenuStartup _mainMenuStartup;
        [SerializeField] private AssetReference _promoButtonAssetReference;

        public override void InstallBindings()
        {
            Container.Bind<MainMenuPlayButtonTemp>()
                .FromInstance(_temp)
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
