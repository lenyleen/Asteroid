using _Project.Scripts.AssetLoaders;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Installers
{
    public class GameplayInitSceneInstaller : MonoInstaller<GameplayInitSceneInstaller>
    {
        [SerializeField] private GameplayAssetsLoader _gameplayAssetsLoader;

        public override void InstallBindings()
        {
            Container.Bind<GameplayAssetsLoader>()
                .FromInstance(_gameplayAssetsLoader)
                .AsSingle();
        }
    }
}
