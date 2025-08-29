using System.Collections.Generic;
using System.Threading.Tasks;
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
        private UnityServicesInstaller _unityServicesInstaller;

        [Inject]
        public void Construct(List<IBootstrapInitializable> bootstrapInitializables,IPopUpService popUpService,
            List<IProjectImportanceInitializable>  projectImportanceInitializables, IProjectAssetProvider assetProvider,
           UnityServicesInstaller unityServicesInstaller)
        {
            _projectImportanceInitializables = projectImportanceInitializables;
            _bootstrapInitializables = bootstrapInitializables;
            _assetProvider = assetProvider;
            _unityServicesInstaller = unityServicesInstaller;
        }

        public async void Start()
        {
            var projectContainer = ProjectContext.Instance.Container;

            await InitializeImportantBindings(projectContainer);

            await _unityServicesInstaller.InitializeAsync();

            await InitializeInitializables(_projectImportanceInitializables);

            await InitializeInitializables(_bootstrapInitializables);

            await _sceneLoader.LoadScene(Scenes.MainMenu);
        }

        private async UniTask InitializeInitializables<T>(List<T> initializables) where T : IAsyncInitializable
        {
            foreach (var asyncInitializable in initializables)
                await asyncInitializable.InitializeAsync();
        }

        private async UniTask InitializeImportantBindings(DiContainer container)
        {
            var projectContextContainer = ProjectContext.Instance.Container;

            var loadCurtainPrefab = (await _assetProvider.Load<GameObject>(LoadCurtainPrefabAddress))
                .GetComponent<LoadCurtain>();

            var loadCurtain = Instantiate(loadCurtainPrefab);
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
        }
    }
}
