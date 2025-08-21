using System;
using System.Collections.Generic;
using _Project.Scripts.Data;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Services;
using _Project.Scripts.UI.PopUps;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Other
{
    public class MainMenuStartup : MonoBehaviour
    {
        private List<ISceneInitializable> _sceneInitializables;
        private UiService _uiService;

        [Inject]
        public void Construct(List<ISceneInitializable> initializables, UiService uiService)
        {
            _sceneInitializables = initializables;
            _uiService = uiService;
        }

        private async void Start()
        {
            try
            {
                foreach (var initializable in _sceneInitializables)
                    await initializable.InitializeAsync();
            }
            catch (Exception e)
            {
                var popUp =
                    _uiService.ShowDialogAwaitable<ErrorPopUp, ErrorPopUpData>(
                        new ErrorPopUpData(e.Message));
            }
        }
    }
}
