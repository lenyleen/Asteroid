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
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Installers
{
    public class UiInstaller : MonoInstaller<UiInstaller>
    {
        [SerializeField] private WeaponUiDataDisplayer _dataDisplayerPrefab;
        [SerializeField] private Transform _popUpParent;
        [SerializeField] private PopUpsConfig _popUpsConfig;
        [SerializeField] private HudUi _hudUi;

        public override void InstallBindings()
        {
            Container.Bind<PopUpFactory>()
                .AsSingle()
                .WithArguments(_popUpsConfig, _popUpParent);

            Container.Bind<UiService>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<WeaponUiDataDisplayerFactory>()
                .AsSingle()
                .WithArguments(_dataDisplayerPrefab);

            Container.BindInterfacesAndSelfTo<WeaponUiDisplayerViewModel>()
                .AsSingle();

            Container.Bind<WeaponUiDisplayerView>()
                .FromComponentInNewPrefab(_hudUi.WeaponUiDisplayerView)
                .AsSingle();

            Container.BindInterfacesAndSelfTo<ScoreBoxModel>()
                .AsSingle();

            Container.Bind<ScoreBoxView>()
                .FromComponentInNewPrefab(_hudUi.ScoreBoxView)
                .AsSingle();

            Container.BindInterfacesAndSelfTo<ShipInfoViewModel>()
                .AsSingle();

            Container.Bind<ShipInfoView>()
                .FromComponentInNewPrefab(_hudUi.ShipInfoView)
                .AsSingle();

            Container.BindInterfacesAndSelfTo<TutorialViewModel>()
                .AsSingle();

            Container.Bind<TutorialView>()
                .FromComponentInNewPrefab(_hudUi.TutorialView)
                .AsSingle();
        }

        [Serializable]
        public class HudUi
        {
            [field: SerializeField] public ShipInfoView ShipInfoView { get; private set; }
            [field: SerializeField] public WeaponUiDisplayerView WeaponUiDisplayerView { get; private set; }
            [field: SerializeField] public ScoreBoxView ScoreBoxView { get; private set; }
            [field: SerializeField] public TutorialView TutorialView { get; private set; }
        }
    }
}
