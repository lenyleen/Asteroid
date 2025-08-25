using System.Collections.Generic;
using _Project.Scripts.Configs;
using _Project.Scripts.Installers;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Services;
using _Project.Scripts.Static;
using _Project.Scripts.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
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
        private SaveCheckHandler  _saveCheckHandler;

        [Inject]
        public void Construct(List<IBootstrapInitializable> bootstrapInitializables,UiService uiService,
            List<IProjectImportanceInitializable>  projectImportanceInitializables, IProjectAssetProvider assetProvider,
            SaveCheckHandler saveCheckHandler)
        {
            _projectImportanceInitializables = projectImportanceInitializables;
            _bootstrapInitializables = bootstrapInitializables;
            _assetProvider = assetProvider;
            _saveCheckHandler = saveCheckHandler;
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

            await CheckLoadServices(projectContextContainer);

            await _sceneLoader.LoadScene(Scenes.MainMenu);
        }

        private async UniTask CheckLoadServices(DiContainer projectContainer)
        {
            var selectedSaveService = await _saveCheckHandler.SelectSaveService();

            projectContainer.Bind<ISaveService>()
                .FromInstance(selectedSaveService)
                .AsSingle();

            Debug.Log($"Selected {selectedSaveService.GetType()}");
        }

        private async UniTask InitializeInitializables<T>(List<T> initializables) where T : IAsyncInitializable
        {
            foreach (var asyncInitializable in initializables)
                await asyncInitializable.InitializeAsync();
        }
    }
}
