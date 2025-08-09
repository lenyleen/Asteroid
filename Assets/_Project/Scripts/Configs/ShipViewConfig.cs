using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Configs
{
    [CreateAssetMenu (fileName = "ShipViewConfig", menuName = "ScriptableObject/ShipViewConfig")]
    public class ShipViewConfig : ScriptableObject
    {
        [field: SerializeField] public AssetReferenceSprite ShipSprite { get; private set; }
        [field: SerializeField] public List<Vector3> HeavyWeaponSlots { get; private set; }
        [field: SerializeField] public List<Vector3> LightWeaponSlots { get; private set; }
    }
}
