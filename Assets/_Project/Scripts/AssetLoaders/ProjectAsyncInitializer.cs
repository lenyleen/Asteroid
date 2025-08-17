using System.Collections.Generic;
using _Project.Scripts.Configs;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Services;
using _Project.Scripts.Static;
using _Project.Scripts.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.AssetLoaders
{
    public class ProjectAsyncInitializer : MonoBehaviour
    {
        private const string AddressesAddress = "GameplayAssetsAddresses";
        private const string LoadCurtainPrefabAddress = "LoadCurtain";

        private SceneLoader _sceneLoader;
        private IAdvertisementService _advertisementService;
        private List<IBootstrapInitializable> _bootstrapInitializables;
        private List<IProjectImportanceInitializable> _projectImportanceInitializables;
        private IProjectAssetProvider  _assetProvider;

        [Inject]
        public void Construct(List<IBootstrapInitializable> bootstrapInitializables,
            List<IProjectImportanceInitializable>  projectImportanceInitializables, IProjectAssetProvider assetProvider)
        {
            _projectImportanceInitializables = projectImportanceInitializables;
            _bootstrapInitializables = bootstrapInitializables;
            _assetProvider = assetProvider;
        }

        public async void Start()
        {
            var projectContextContainer = ProjectContext.Instance.Container;

            var loadCurtainPrefab = (await _assetProvider.Load<GameObject>(LoadCurtainPrefabAddress))
                .GetComponent<LoadCurtain>();

            var loadCurtain = Instantiate<LoadCurtain>(loadCurtainPrefab);
            DontDestroyOnLoad(loadCurtain);

            loadCurtain.gameObject.SetActive(false);

            projectContextContainer.Bind<LoadCurtain>()
                .FromInstance(loadCurtain)
                .AsSingle();

            var gameplayAssetsAddresses = await _assetProvider.Load<GameplayAssetsAddresses>(AddressesAddress);

            projectContextContainer.Bind<GameplayAssetsAddresses>()
                .FromInstance(gameplayAssetsAddresses)
                .AsSingle();

            _sceneLoader = new SceneLoader(loadCurtain);
            projectContextContainer.Bind<SceneLoader>()
                .AsSingle();

            await InitializeInitializables(_projectImportanceInitializables);
            await InitializeInitializables(_bootstrapInitializables);

            await _sceneLoader.LoadScene(Scenes.MainMenu);
        }

        private async UniTask InitializeInitializables<T>(List<T> initializables) where T : IAsyncInitializable
        {
            foreach (var asyncInitializable in initializables)
                await asyncInitializable.InitializeAsync();
        }
    }
}
