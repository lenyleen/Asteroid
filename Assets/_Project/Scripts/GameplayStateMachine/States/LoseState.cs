using System;
using _Project.Scripts.Data;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Other;
using _Project.Scripts.Services;
using _Project.Scripts.Static;
using _Project.Scripts.UI;
using _Project.Scripts.UI.PopUps;
using _Project.Scripts.UI.ScoreBox;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace _Project.Scripts.GameplayStateMachine.States
{
    public class LoseState : IState
    {
        public IObservable<Type> OnStateChanged => _changeStateCommand;

        private readonly IPopUpService _popUpService;
        private readonly ScoreBoxModel _scoreModel;
        private readonly PlayerProgressProvider _playerProgressProvider;
        private readonly ReactiveCommand<Type> _changeStateCommand = new();
        private readonly IScenesAssetProvider _assetProvider;
        private readonly IAnalyticsService _analyticsService;
        private readonly IAdvertisementService _advertisementService;
        private readonly LoadCurtain _loadCurtain;
        private readonly SceneLoader _sceneLoader;

        private CompositeDisposable _disposables = new();

        public LoseState(IPopUpService popUpService, ScoreBoxModel scoreModel, PlayerProgressProvider playerProgressProvider,
            IScenesAssetProvider assetProvider, IAnalyticsService analyticsService, LoadCurtain loadCurtain,
            SceneLoader sceneLoader,
            IAdvertisementService advertisementService)
        {
            _popUpService = popUpService;
            _scoreModel = scoreModel;
            _playerProgressProvider = playerProgressProvider;
            _assetProvider = assetProvider;
            _analyticsService = analyticsService;
            _loadCurtain = loadCurtain;
            _sceneLoader = sceneLoader;
            _advertisementService = advertisementService;
        }

        public async void Enter()
        {
            _analyticsService.SendEndGameAnalytics();
            _scoreModel.Enable(false);

            var restartDialog = await _popUpService
                .ShowDialogAwaitable<LosePopUp, LosePopUpData>(new LosePopUpData(_scoreModel.Score.Value));

            var restartDialogResult = await restartDialog.ShowDialogAsync(true);

            try
            {
                await _playerProgressProvider.SetDataAsync();
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message + e.StackTrace);
                await ThrowError(e.Message);
                return;
            }

            await Restart(restartDialogResult);
        }

        public void Exit()
        {
            _disposables.Dispose();
            _disposables = new CompositeDisposable();
        }

        private async UniTask Restart(DialogResult result)
        {
            try
            {
                if (result != DialogResult.Yes)
                {
                    await _advertisementService.ShowInterstitial();
                    await ToMainMenu();
                    return;
                }

                var adResult = await _advertisementService.ShowRewarded();
                if (!adResult)
                {
                    await ToMainMenu();
                    return;
                }

                ToGameplay();
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.Message + e.StackTrace);
                await ThrowError(e.Message);
                await ToMainMenu();
            }
        }

        private void ToGameplay()
        {
            _playerProgressProvider.ToDefault();
            _changeStateCommand.Execute(typeof(PlayState));
        }

        private async UniTask ToMainMenu()
        {
            _assetProvider.Dispose();
            await _sceneLoader.LoadSceneWithCurtain(Scenes.MainMenu);
            await _loadCurtain.FadeOutAsync();
        }

        private async UniTask ThrowError(string message)
        {
            await _popUpService.ShowDialogAwaitable<ErrorPopUp, ErrorPopUpData>(new ErrorPopUpData(message));
            await ToMainMenu();
        }
    }
}
