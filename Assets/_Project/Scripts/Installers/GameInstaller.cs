using _Project.Scripts.GameplayStateMachine.States;
using _Project.Scripts.Input;
using _Project.Scripts.Services;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Installers
{
    public class GameInstaller : MonoInstaller<GameInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<Camera>().FromInstance(Camera.main)
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

            Container.BindInterfacesAndSelfTo<GameplayStateMachine.GameplayStateMachine>()
                .AsSingle();

            Container.Bind<FirebaseAnalyticsService>().AsSingle();
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
