using _Project.Scripts;
using _Project.Scripts.States;
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

            Container.Bind<AssetProvider>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<SaveLoadService>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerProgressProvider>()
                .AsSingle();

            Container.Bind<GameInput>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerInputController>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<ScreenWrapService>()
                .AsSingle();

            BindStates();

            Container.BindInterfacesAndSelfTo<GameplayStateMachine>()
                .AsSingle();
        }

        private void BindStates()
        {
            Container.BindInterfacesAndSelfTo<InputWaitState>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<PlayState>()
                .AsSingle();

            Container.BindInterfacesAndSelfTo<LoseState>()
                .AsSingle();
        }
    }
}
