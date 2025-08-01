using System;
using _Project.Scripts.GameplayStateMachine.States;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Services;
using _Project.Scripts.UI;
using _Project.Scripts.UI.PopUps;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Other
{
    public class GameStartup : MonoBehaviour
    {
        [SerializeField] private LoadCurtain _loadCurtain;

        private UiService _uiService;
        private PlayerProgressProvider _playerProgressProvider;
        private GameplayStateMachine.GameplayStateMachine _gameplayStateMachine;
        private IAnalyticsService _analyticsService;

        [Inject]
        private void Construct(PlayerProgressProvider playerProgressProvider,
            GameplayStateMachine.GameplayStateMachine gameplayStateMachine,
            UiService uiService, IAnalyticsService firebaseAnalytics)
        {
            _playerProgressProvider = playerProgressProvider;
            _gameplayStateMachine = gameplayStateMachine;
            _uiService = uiService;
            _analyticsService = firebaseAnalytics;
        }

        private async void Start()
        {
            try
            {
                await _playerProgressProvider.TryInitializeAsync();
                await _loadCurtain.FadeOutAsync();
                await _analyticsService.InitializeAsync();

                _gameplayStateMachine.Initialize(typeof(InputWaitState));
            }
            catch (Exception e)
            {
                await _uiService.ShowDialogAwaitable<ErrorPopUp, string, DialogResult>(e.Message);

                //goto main menu
            }
        }
    }
}
