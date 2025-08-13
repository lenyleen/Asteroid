using System;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Interfaces
{
    public interface IAdvertisementService
    {
        public IObservable<bool> CanShowRewardedAds { get; }
        public IObservable<bool> CanShowInterstitialAds { get; }

        public UniTask InitializeAsync();
        public UniTask<bool> ShowRewarded();
        public UniTask ShowInterstitial();
    }
}
