using System;
using System.Collections.Generic;
using _Project.Scripts.Data;
using _Project.Scripts.Factories;
using _Project.Scripts.GameplayStateMachine.States;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Services;
using _Project.Scripts.Static;
using _Project.Scripts.UI.PopUps;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Other
{
    public class GameStartup : MonoBehaviour
    {
        private IPopUpService _popUpService;
        private GameplayStateMachine.GameplayStateMachine _gameplayStateMachine;
        private SceneLoader _sceneLoader;
        private List<ISceneInitializable> _initializables;
        private BgmHandlerFactory _bgmHandlerFactory;

        [Inject]
        private void Construct(List<ISceneInitializable> initializables,BgmHandlerFactory bgmHandlerFactory,
            GameplayStateMachine.GameplayStateMachine gameplayStateMachine,
            IPopUpService popUpService, SceneLoader sceneLoader)
        {
            _initializables = initializables;
            _gameplayStateMachine = gameplayStateMachine;
            _popUpService = popUpService;
            _sceneLoader = sceneLoader;
            _bgmHandlerFactory = bgmHandlerFactory;
        }

        private async void Start()
        {
            try
            {
                foreach (var initializable in _initializables)
                    await initializable.InitializeAsync();

                await _bgmHandlerFactory.Create();
                await _sceneLoader.FadeOut();

                _gameplayStateMachine.Initialize(typeof(InputWaitState));
            }
            catch (Exception e)
            {
                await _popUpService.ShowDialogAwaitable<ErrorPopUp, ErrorPopUpData>(new ErrorPopUpData(e.Message));

                Debug.Log(e);

                await _sceneLoader.LoadScene(Scenes.MainMenu);
            }
        }
    }
}
