using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(fileName = "GameplayAssetsAddresses", menuName = "ScriptableObject/GameplayAssetsAddresses")]
    public class GameplayAssetsAddresses : ScriptableObject
    {
        [field: SerializeField] public List<AssetReference> AssetReferences { get; private set; }
    }
}
