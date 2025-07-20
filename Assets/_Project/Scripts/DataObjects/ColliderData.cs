using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DataObjects
{
    [CreateAssetMenu(fileName = "ColliderData", menuName = "ScriptableObject/ColliderData")]
    public class ColliderData : ScriptableObject
    {
        [field:SerializeField] public ColliderType ColliderType;
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public List<ColliderType> AcceptableColliderTypes { get; private set; }
    }
}