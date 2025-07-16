using Services;
using Zenject;

namespace Installers
{
    public class GameInstaller : Installer<GameInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<GameInput>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerInputController>().AsSingle();
            /*Container.BindInterfacesAndSelfTo<ScreenWrapService>().AsSingle();*/
        }
    }
}