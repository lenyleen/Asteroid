using System;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Interfaces
{
    public interface IAdvertisementService : IBootstrapInitializable
    {
        public IObservable<bool> CanShowRewardedAds { get; }
        public UniTask<bool> ShowRewarded();
        public UniTask ShowInterstitial();
    }
}
