using System.Collections.Generic;
using UnityEngine;

namespace DataObjects
{
    [CreateAssetMenu(fileName = "ColliderData", menuName = "ScrtiptableObject/ColliderData")]
    public class ColliderData : ScriptableObject
    {
        [field:SerializeField] public ColliderType colliderType;
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public List<ColliderType> AcceptableColliderTypes { get; private set; }
    }
}