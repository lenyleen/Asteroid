using UnityEngine;

namespace _Project.Scripts.Configs
{
    public class UnityAdvertisementsConfig : ScriptableObject
    {
        [field: SerializeField] public string AndroidGameId { get; private set; } = "5922168";
        [field: SerializeField] public string RewardedAdId { get; private set; } = "Rewarded_Android";
        [field: SerializeField] public string InterstitialAdId { get; private set; } = "Interstitial_Android";
    }
}
