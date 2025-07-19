using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DataObjects
{
    [CreateAssetMenu(fileName = "ColliderData", menuName = "ScrtiptableObject/ColliderData")]
    public class ColliderData : ScriptableObject
    {
        [FormerlySerializedAs("colliderType")] [field:SerializeField] public ColliderType ColliderType;
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public List<ColliderType> AcceptableColliderTypes { get; private set; }
    }
}