using _Project.Scripts.Configs;
using _Project.Scripts.Services;
using _Project.Scripts.Static;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.AssetLoaders
{
    public class GameplayAssetsLoader : MonoBehaviour
    {
        private GameplayAssetsAddresses _gameplayAssetsAddresses;
        private SceneLoader _sceneLoader;
        private AssetProvider _assetProvider;

        [Inject]
        public void Construct(SceneLoader sceneLoader, AssetProvider assetProvider, GameplayAssetsAddresses gameplayAssetsAddresses)
        {
            _sceneLoader = sceneLoader;
            _assetProvider = assetProvider;
            _gameplayAssetsAddresses = gameplayAssetsAddresses;
        }

        public async void Awake()
        {
            Debug.Log($"SceneLoader exists? {ProjectContext.Instance.Container.HasBinding<SceneLoader>()}");

            await LoadAssetsAsync();

            await _sceneLoader.LoadScene(Scenes.Gameplay);
        }

        private async UniTask LoadAssetsAsync()
        {
            foreach (var reference in _gameplayAssetsAddresses.AssetReferences)
                await _assetProvider.Load<GameObject>(reference);
        }
    }
}
