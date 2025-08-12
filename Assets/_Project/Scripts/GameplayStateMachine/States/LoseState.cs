using System;
using _Project.Scripts.Interfaces;
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
        private readonly AssetProvider _assetProvider;
        private readonly IAnalyticsService _analyticsService;

        private CompositeDisposable _disposables = new();

        public LoseState(UiService uiService, ScoreBoxModel scoreModel, PlayerProgressProvider playerProgressProvider,
            AssetProvider assetProvider,  IAnalyticsService analyticsService)
        {
            _uiService = uiService;
            _scoreModel = scoreModel;
            _playerProgressProvider = playerProgressProvider;
            _assetProvider = assetProvider;
            _analyticsService = analyticsService;
        }

        public async void Enter()
        {
            _analyticsService.SendEndGameAnalytics();
            _scoreModel.Enable(false);

            var restartDialogResult = await _uiService
                .ShowDialogAwaitable<LosePopUp, int, DialogResult>(_scoreModel.Score.Value);

            try
            {
                await _playerProgressProvider.SetDataAsync();
            }
            catch (Exception e)
            {
                await _uiService.ShowDialogAwaitable<ErrorPopUp, string, DialogResult>(e.Message);
                _assetProvider.Dispose();
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
            {
                _assetProvider.Dispose();
                return; //переход в главное меню
            }

            _playerProgressProvider.ToDefault();
            _changeStateCommand.Execute(typeof(PlayState));
        }
    }
}
