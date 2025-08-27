using _Project.Scripts.Services;
using _Project.Scripts.Static;
using _Project.Scripts.UI.PopUps;
using UniRx;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.Scripts.MainMenu
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _exitButton;

        private SceneLoader _sceneLoader;
        private UiService _uiService;

        [Inject]
        private void Construct(UiService uiService, SceneLoader sceneLoader)
        {
            _uiService = uiService;
            _sceneLoader = sceneLoader;
        }

        private void Awake()
        {
            _playButton.OnClickAsObservable().Subscribe(async _ =>
                    await _sceneLoader.LoadSceneWithCurtain(Scenes.GameplayInitial))
                .AddTo(this);

            _settingsButton.OnClickAsObservable().Subscribe(_ =>
                    _uiService.ShowPopUp<SettingsPopUp>())
                .AddTo(this);

            _exitButton.OnClickAsObservable().Subscribe(_ =>
            {
            #if UNITY_EDITOR
                EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
            });
        }
    }
}
