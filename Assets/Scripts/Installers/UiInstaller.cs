using Factories;
using UI;
using Zenject;

namespace Installers
{
    public class UiInstaller : Installer<InGameUi, WeaponUiDataDisplayer, PlayerUiView,UiInstaller>
    {
        private InGameUi _inGameUi;
        private WeaponUiDataDisplayer  _dataDisplayerPrefab;
        private PlayerUiView _playerUi;

        public UiInstaller(InGameUi inGameUi, WeaponUiDataDisplayer dataDisplayerPrefab, PlayerUiView playerUi)
        {
            _inGameUi = inGameUi;
            _dataDisplayerPrefab = dataDisplayerPrefab;
            _playerUi = playerUi;
        }
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