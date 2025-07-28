using System;
using _Project.Scripts.DTO;
using Other;
using Services;
using UI;
using UI.PopUps;
using UI.ScoreBox;
using UniRx;

namespace _Project.Scripts.States
{
    public class LoseState : IState
    {
        private readonly UiService _uiService;
        private readonly GameplayStateMachine _stateMachine;
        private readonly ScoreBoxModel _scoreModel;
        private readonly PlayerDataProvider _playerDataProvider;

        private CompositeDisposable _disposables = new();

        public LoseState(UiService uiService, GameplayStateMachine stateMachine,
            ScoreBoxModel scoreModel, PlayerDataProvider playerDataProvider)
        {
            _uiService = uiService;
            _stateMachine = stateMachine;
            _scoreModel = scoreModel;
            _playerDataProvider = playerDataProvider;
        }

        public async void Enter()
        {
            var restartDialogResult = await _uiService.ShowDialogAwaitable<LosePopUp, int, DialogResult>(10);

            try
            {
                await _playerDataProvider.TrySetDataAsync(new PlayerData { Score = _scoreModel.Score.Value });
            }
            catch (Exception e)
            {
                await _uiService.ShowDialogAwaitable<ErrorPopUp, string, DialogResult>(e.Message);
                return; //типа переход в главное меню
            }

            Restart(restartDialogResult);
        }

        public void Exit()
        {
            _disposables.Dispose();
            _disposables = new CompositeDisposable();
        }

        private void Restart(DialogResult result)
        {
            if (result != DialogResult.Yes)
                return; //переход в главное меню

            _stateMachine.ChangeState<PlayState>();
        }
    }
}
