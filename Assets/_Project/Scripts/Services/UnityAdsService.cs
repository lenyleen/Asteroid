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
        private const string AdsConfigAddress = "UnityAdsConfig";

        public IObservable<bool> CanShowRewardedAds => _canShowRewardedAds;
        public IObservable<bool> CanShowInterstitialAds => _canShowInterstitialAds;

        private readonly AssetProvider  _assetProvider;
        private readonly UnityAdsShowListener _showListener;
        private readonly ReactiveProperty<bool> _canShowRewardedAds = new (false);
        private readonly ReactiveProperty<bool> _canShowInterstitialAds = new (false);

        private UnityAdvertisementsConfig _config;

        public UnityAdsService(AssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public async UniTask InitializeAsync()
        {
            _config =
                await _assetProvider.Load<UnityAdvertisementsConfig>(AdsConfigAddress);

            Advertisement.Initialize(_config.AndroidGameId,true,this);
        }

        public void OnInitializationComplete()
        {
            _canShowRewardedAds.Value = true;
            _canShowInterstitialAds.Value = true;
        }

        public async UniTask<bool> ShowRewarded()
        {
            if (_canShowRewardedAds.Value)
                return false;
            EnableAdShowing(false);

            var listener = await LoadAd(_config.RewardedAdId);

            var result = await listener.ShowAsync(_config.RewardedAdId);

            EnableAdShowing(true);

            return result == UnityAdsShowCompletionState.COMPLETED;
        }

        public async UniTask ShowInterstitial()
        {
            if(_canShowInterstitialAds.Value)
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
            _canShowInterstitialAds.Value = enable;
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message){}
    }
}
