using System;
using _Project.Scripts;
using _Project.Scripts.States;
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
        private PlayerDataProvider _playerDataProvider;
        private GameplayStateMachine _gameplayStateMachine;

        [Inject]
        private void Construct(PlayerDataProvider playerDataProvider, GameplayStateMachine gameplayStateMachine,
            UiService uiService)
        {
            _playerDataProvider = playerDataProvider;
            _gameplayStateMachine = gameplayStateMachine;
            _uiService = uiService;
        }

        private async void Start()
        {
            try
            {
                await _playerDataProvider.TryInitializeAsync();
                await _loadCurtain.FadeOutAsync();
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
