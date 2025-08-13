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

        [Inject]
        public void Construct(SceneLoader sceneLoader, IAdvertisementService advertisementService)
        {
            _sceneLoader = sceneLoader;
            _advertisementService = advertisementService;
        }

        public async void Start()
        {
            var projectContextContainer = ProjectContext.Instance.Container;

            var loadCurtain = (await Addressables
                .InstantiateAsync(LoadCurtainPrefabAddress).ToUniTask()).GetComponent<LoadCurtain>();
            DontDestroyOnLoad(loadCurtain);
            loadCurtain.gameObject.SetActive(false);

            projectContextContainer.Bind<LoadCurtain>()
                .FromInstance(loadCurtain)
                .AsSingle();

            var gameplayAssetsAddresses = await Addressables
                .LoadAssetAsync<GameplayAssetsAddresses>(AddressesAddress).ToUniTask();

            projectContextContainer.Bind<GameplayAssetsAddresses>()
                .FromInstance(gameplayAssetsAddresses)
                .AsSingle();

            await _advertisementService.InitializeAsync();

            await _sceneLoader.LoadScene(Scenes.MainMenu);
        }
    }
}
