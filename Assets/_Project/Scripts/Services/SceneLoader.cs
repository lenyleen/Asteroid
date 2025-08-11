using System;
using _Project.Scripts.UI;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using Zenject;

namespace _Project.Scripts.Services
{
    public class SceneLoader
    {
        public async UniTask LoadScene(string sceneName)
        {
            await SceneManager.LoadSceneAsync(sceneName)
                .ToUniTask();
        }
    }
}
