using System;
using _Project.Scripts.Configs;
using _Project.Scripts.Factories;
using _Project.Scripts.Services;
using _Project.Scripts.UI;
using _Project.Scripts.UI.ScoreBox;
using _Project.Scripts.UI.ShipInfoInfo;
using _Project.Scripts.UI.Tutorial;
using _Project.Scripts.UI.WeaponUi;
using Factories;
using Services;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.Installers
{
    public class UiInstaller : MonoInstaller<UiInstaller>
    {
        [SerializeField] private Transform _popUpParent;
        [SerializeField] private Transform _hudParent;
        [SerializeField] private PopUpsConfig _popUpsConfig;

        [Header("Asset References")]
        [SerializeField] private AssetReference _weaponDataDisplayerPrefabReference;
        [SerializeField] private AssetReference _hudPrefabReference;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PopUpFactory>()
                .AsSingle()
                .WithArguments(_popUpsConfig, _popUpParent);

            Container.Bind<UiService>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<WeaponUiDataDisplayerFactory>()
                .AsSingle()
                .WithArguments(_weaponDataDisplayerPrefabReference);

            Container.BindInterfacesAndSelfTo<HudFactory>()
                .AsSingle()
                .WithArguments(_hudPrefabReference, _hudParent);

            Container.BindInterfacesAndSelfTo<WeaponUiDisplayerViewModel>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<ScoreBoxModel>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<ShipInfoViewModel>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<TutorialViewModel>()
                .AsSingle();
        }
    }
}
