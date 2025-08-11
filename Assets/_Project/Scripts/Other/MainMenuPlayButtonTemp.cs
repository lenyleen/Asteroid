using System;
using _Project.Scripts.Services;
using _Project.Scripts.UI;
using Static;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.Scripts.Other
{
    public class MainMenuPlayButtonTemp : MonoBehaviour
    {
        [SerializeField] private Button _playButton;

        private SceneLoader _sceneLoader;
        private LoadCurtain _loadCurtain;

        [Inject]
        private void Construct(SceneLoader sceneLoader, LoadCurtain loadCurtain)
        {
            _loadCurtain = loadCurtain;
            _sceneLoader = sceneLoader;
        }

        private void Start()
        {
            _playButton.onClick.AddListener(Play);
        }

        private async void Play()
        {
            try
            {
                await _loadCurtain.FadeInAsync();
                await _sceneLoader.LoadScene(Scenes.GameplayInitial);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load gameplay scene: {e.Message}");
            }
        }
    }
}
