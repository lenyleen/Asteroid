using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Configs
{
    [CreateAssetMenu(fileName = "PopUpsConfig", menuName = "ScriptableObject/PopUpsConfig")]
    public class PopUpsConfig : ScriptableObject
    {
        [field: SerializeField] public List<AssetReferenceGameObject> PopUpPrefabsReferences { get; private set; }
    }
}
