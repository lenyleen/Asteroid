using System;
using _Project.Scripts.Configs;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Other;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.Advertisements;

namespace _Project.Scripts.Services
{
    public class UnityAdsService : IUnityAdsInitializationListener, IAdvertisementService
    {
        private const string AdsRemoteConfigKey = "UnityAdsConfig";

        public IObservable<bool> CanShowRewardedAds => _canShowRewardedAds;

        private readonly IRemoteConfigService _remoteConfigService;
        private readonly UnityAdsShowListener _showListener;
        private readonly ReactiveProperty<bool> _canShowRewardedAds = new (false);

        private UnityAdvertisementsConfig _config;
        private bool _canShowInterstitialAds;

        public UnityAdsService(IRemoteConfigService remoteConfigService)
        {
            _remoteConfigService = remoteConfigService;
        }

        public UniTask InitializeAsync()
        {
            _config = _remoteConfigService.GetConfig<UnityAdvertisementsConfig>(AdsRemoteConfigKey);

            Advertisement.Initialize(_config.AndroidGameId,true,this);

            return UniTask.CompletedTask;
        }

        public void OnInitializationComplete()
        {
            _canShowRewardedAds.Value = true;
            _canShowInterstitialAds = true;
        }

        public async UniTask<bool> ShowRewarded()
        {
            if (!_canShowRewardedAds.Value)
                return false;
            EnableAdShowing(false);

            var listener = await LoadAd(_config.RewardedAdId);

            var result = await listener.ShowAsync(_config.RewardedAdId);

            EnableAdShowing(true);

            return result == UnityAdsShowCompletionState.COMPLETED;
        }

        public async UniTask ShowInterstitial()
        {
            if(!_canShowInterstitialAds)
                return;

            EnableAdShowing(false);

            var listener = await LoadAd(_config.InterstitialAdId);

            EnableAdShowing(true);

            await listener.ShowAsync(_config.InterstitialAdId);
        }

        private async UniTask<UnityAdsShowListener> LoadAd(string placementId)
        {
            var listener =  new UnityAdsShowListener();
            await listener.LoadAsync(placementId);

            return listener;
        }

        private void EnableAdShowing(bool enable)
        {
            _canShowRewardedAds.Value = enable;
            _canShowInterstitialAds = enable;
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.LogWarning("Ads initialization failed");
        }
    }
}
