using _Project.Scripts.AssetLoaders;
using _Project.Scripts.Configs;
using _Project.Scripts.Services;
using _Project.Scripts.UI;
using Configs;
using Cysharp.Threading.Tasks;
using Services;
using Static;
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
