using Services;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GameInstaller : MonoInstaller<GameInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<Camera>().FromInstance(Camera.main)
                .AsSingle();

            Container.Bind<GameInput>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerInputController>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<ScreenWrapService>()
                .AsSingle();
        }
    }
}
