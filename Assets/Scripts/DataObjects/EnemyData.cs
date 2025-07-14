using Enemy;
using UnityEngine;

namespace DataObjects
{
    public class EnemyData : ScriptableObject
    {
        [field:SerializeField] public Sprite Sprite { get; private set; }
        [field:SerializeField] public int Health { get; private set; }
        [field:SerializeField] public EnemyType Type { get; private set; }
    }
}