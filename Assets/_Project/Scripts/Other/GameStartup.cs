using System;
using _Project.Scripts;
using _Project.Scripts.States;
using Factories;
using Services;
using UI;
using UI.PopUps;
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

        [Inject]
        private void Construct(PlayerProgressProvider playerProgressProvider, GameplayStateMachine gameplayStateMachine,
            UiService uiService, PopUpFactory popUpFactory)
        {
            _playerProgressProvider = playerProgressProvider;
            _gameplayStateMachine = gameplayStateMachine;
            _uiService = uiService;
            _popUpFactory = popUpFactory;
        }

        private async void Start()
        {
            try
            {
                await _playerProgressProvider.TryInitializeAsync();
                await _loadCurtain.FadeOutAsync();
                await _popUpFactory.InitializePopUpsAsync();
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
