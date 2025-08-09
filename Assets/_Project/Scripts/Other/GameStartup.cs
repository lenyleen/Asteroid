using System;
using _Project.Scripts.GameplayStateMachine;
using _Project.Scripts.GameplayStateMachine.States;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Other;
using _Project.Scripts.UI;
using _Project.Scripts.Services;
using _Project.Scripts.UI.PopUps;
using Factories;
using UnityEngine;
using Zenject;

namespace Other
{
    public class GameStartup : MonoBehaviour
    {
        [SerializeField] private LoadCurtain _loadCurtain;

        private UiService _uiService;
        private PlayerProgressProvider _playerProgressProvider;
        private GameplayStateMachine _gameplayStateMachine;
        private PopUpFactory _popUpFactory;
        private IAnalyticsService _analyticsService;

        [Inject]
        private void Construct(PlayerProgressProvider playerProgressProvider, GameplayStateMachine gameplayStateMachine,
            UiService uiService, PopUpFactory popUpFactory,IAnalyticsService firebaseAnalytics)
        {
            _playerProgressProvider = playerProgressProvider;
            _gameplayStateMachine = gameplayStateMachine;
            _uiService = uiService;
            _popUpFactory = popUpFactory;
            _analyticsService = firebaseAnalytics;
        }

        private async void Start()
        {
            try
            {
                await _playerProgressProvider.TryInitializeAsync();
                await _loadCurtain.FadeOutAsync();
                await _popUpFactory.InitializePopUpsAsync();
                await _analyticsService.InitializeAsync();

                _gameplayStateMachine.Initialize(typeof(InputWaitState));
            }
            catch (Exception e)
            {
                await _uiService.ShowDialogAwaitable<ErrorPopUp,string,DialogResult>(e.Message);

                //goto main menu
            }
        }
    }
}
