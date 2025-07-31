using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(fileName = "ColliderConfig", menuName = "ScriptableObject/ColliderConfig")]
    public class ColliderConfig : ScriptableObject
    {
        [field: SerializeField] public ColliderType ColliderType { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public List<ColliderType> AcceptableColliderTypes { get; private set; }
    }
}
