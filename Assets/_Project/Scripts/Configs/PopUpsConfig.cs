using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(fileName = "PopUpsConfig", menuName = "ScriptableObject/PopUpsConfig")]
    public class PopUpsConfig : ScriptableObject
    {
        [field: SerializeField] public List<GameObject> PopUpPrefabs { get; private set; }
    }
}
