using System;
using _Project.Scripts.Other;
using _Project.Scripts.Services;
using _Project.Scripts.UI.PopUps;
using _Project.Scripts.UI.ScoreBox;
using UniRx;

namespace _Project.Scripts.GameplayStateMachine.States
{
    public class LoseState : IState
    {
        public IObservable<Type> OnStateChanged => _changeStateCommand;

        private readonly UiService _uiService;
        private readonly ScoreBoxModel _scoreModel;
        private readonly PlayerProgressProvider _playerProgressProvider;
        private readonly ReactiveCommand<Type> _changeStateCommand = new();

        private CompositeDisposable _disposables = new();

        public LoseState(UiService uiService, ScoreBoxModel scoreModel, PlayerProgressProvider playerProgressProvider)
        {
            _uiService = uiService;
            _scoreModel = scoreModel;
            _playerProgressProvider = playerProgressProvider;
        }

        public async void Enter()
        {
            _scoreModel.Enable(false);

            var restartDialogResult = await _uiService
                .ShowDialogAwaitable<LosePopUp, int, DialogResult>(_scoreModel.Score.Value);

            try
            {
                await _playerProgressProvider.TrySetDataAsync();
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

            _playerProgressProvider.ToDefault();
            _changeStateCommand.Execute(typeof(PlayState));
        }
    }
}
