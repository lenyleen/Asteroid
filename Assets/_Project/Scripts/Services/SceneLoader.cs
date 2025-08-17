using _Project.Scripts.UI;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Services
{
    public class SceneLoader
    {
        private readonly LoadCurtain  _loadCurtain;

        public SceneLoader(LoadCurtain loadCurtain)
        {
            _loadCurtain = loadCurtain;
        }

        public async UniTask LoadScene(string sceneName)
        {
            await SceneManager.LoadSceneAsync(sceneName)
                .ToUniTask();
        }

        public async UniTask LoadSceneWithCurtain(string sceneName)
        {
            await _loadCurtain.FadeInAsync();

            await LoadScene(sceneName);
        }

        public async UniTask FadeOut()
        {
            await _loadCurtain.FadeOutAsync();
        }
    }
}
