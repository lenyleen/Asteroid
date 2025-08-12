using System;
using System.Collections.Generic;
using _Project.Scripts.GameplayStateMachine.States;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Services;
using _Project.Scripts.Static;
using _Project.Scripts.UI;
using _Project.Scripts.UI.PopUps;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Other
{
    public class GameStartup : MonoBehaviour
    {
        private LoadCurtain _loadCurtain;
        private UiService _uiService;
        private GameplayStateMachine.GameplayStateMachine _gameplayStateMachine;
        private SceneLoader _sceneLoader;
        private List<IAsyncInitializable> _initializables;

        [Inject]
        private void Construct(List<IAsyncInitializable> initializables, GameplayStateMachine.GameplayStateMachine gameplayStateMachine,
            UiService uiService, SceneLoader sceneLoader, LoadCurtain loadCurtain)
        {
            _initializables = initializables;
            _gameplayStateMachine = gameplayStateMachine;
            _uiService = uiService;
            _loadCurtain = loadCurtain;
            _sceneLoader = sceneLoader;
        }

        private async void Start()
        {
            try
            {
                foreach (var initializable in _initializables)
                    await initializable.InitializeAsync();

                await _loadCurtain.FadeOutAsync();

                _gameplayStateMachine.Initialize(typeof(InputWaitState));
            }
            catch (Exception e)
            {
                await _uiService.ShowDialogAwaitable<ErrorPopUp,string,DialogResult>(e.Message);

                await _sceneLoader.LoadScene(Scenes.MainMenu);
            }
        }
    }
}
