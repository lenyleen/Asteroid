using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(fileName = "ShipViewConfig", menuName = "ScriptableObject/ShipViewConfig")]
    public class ShipViewConfig : ScriptableObject
    {
        [field: SerializeField] public string ShipSpriteAdress { get; private set; }
        [field: SerializeField] public List<Vector3> HeavyWeaponSlots { get; private set; }
        [field: SerializeField] public List<Vector3> LightWeaponSlots { get; private set; }
        [field: SerializeField] public WeaponConfig MainWeaponConfig { get; private set; }
        [field: SerializeField] public WeaponConfig HeavyWeaponConfig { get; private set; }
    }
}
