using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(fileName = "PopUpsConfig", menuName = "ScriptableObject/PopUpsConfig")]
    public class PopUpsConfig : ScriptableObject
    {
        [field: SerializeField] public List<AssetReferenceGameObject> PopUpPrefabsReferences { get; private set; }
    }
}
