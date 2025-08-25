using System;
using Newtonsoft.Json;

namespace _Project.Scripts.Configs
{
    [Serializable]
    public class UnityAdvertisementsConfig
    {
        public string AndroidGameId { get; private set; }

        public string RewardedAdId { get; private set; }

        public string InterstitialAdId { get; private set; }

        [JsonConstructor]
        public UnityAdvertisementsConfig(string androidGameId, string rewardedAdId, string interstitialAdId)
        {
            AndroidGameId = androidGameId;
            RewardedAdId = rewardedAdId;
            InterstitialAdId = interstitialAdId;
        }
    }
}
