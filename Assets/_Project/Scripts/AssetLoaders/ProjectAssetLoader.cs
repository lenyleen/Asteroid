using _Project.Scripts.Configs;
using _Project.Scripts.Services;
using _Project.Scripts.UI;
using Cysharp.Threading.Tasks;
using Static;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.AssetLoaders
{
    public class ProjectAssetLoader : MonoBehaviour
    {
        private const string AddressesAddress = "GameplayAssetsAddresses";
        private const string LoadCurtainPrefabAddress = "LoadCurtain";

        private SceneLoader _sceneLoader;

        [Inject]
        public void Construct(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
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

            await _sceneLoader.LoadScene(Scenes.MainMenu);
        }
    }
}
