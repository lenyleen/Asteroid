using Factories;
using UI;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class UiInstaller : MonoInstaller<UiInstaller>
    {
        [SerializeField]private InGameUi _inGameUi;
        [SerializeField]private WeaponUiDataDisplayer  _dataDisplayerPrefab;
        [SerializeField]private PlayerUiView _playerUi;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<WeaponUiDataDisplayerFactory>()
                .AsSingle()
                .WithArguments(_dataDisplayerPrefab);

            Container.BindInterfacesAndSelfTo<InGameUiViewModel>()
                .AsSingle();
            
            Container.BindInterfacesAndSelfTo<InGameUi>()
                .FromInstance(_inGameUi)
                .AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerModel>()
                .AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerViewModel>()
                .AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerUiView>()
                .FromInstance(_playerUi)
                .AsSingle();

        }
    }
}