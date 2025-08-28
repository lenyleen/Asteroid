using System;
using System.Collections.Generic;
using _Project.Scripts.Data;
using _Project.Scripts.Factories;
using _Project.Scripts.Interfaces;
using _Project.Scripts.UI.PopUps;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Other
{
    public class MainMenuStartup : MonoBehaviour
    {
        private List<ISceneInitializable> _sceneInitializables;
        private IPopUpService _popUpService;
        private BgmHandlerFactory _bgmHandlerFactory;

        [Inject]
        public void Construct(List<ISceneInitializable> initializables, IPopUpService popUpService,
            BgmHandlerFactory bgmHandlerFactory)
        {
            _sceneInitializables = initializables;
            _popUpService = popUpService;
            _bgmHandlerFactory = bgmHandlerFactory;
        }

        private async void Start()
        {
            try
            {
                foreach (var initializable in _sceneInitializables)
                    await initializable.InitializeAsync();

                await _bgmHandlerFactory.Create();
            }
            catch (Exception e)
            {
                var popUp =
                    _popUpService.ShowDialogAwaitable<ErrorPopUp, ErrorPopUpData>(
                        new ErrorPopUpData(e.Message));
            }
        }
    }
}
