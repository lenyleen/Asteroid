using UnityEngine;
using Zenject;

namespace Enemy
{
    public class Enemy : MonoBehaviour
    {
        public class Pool : MonoMemoryPool<Enemy>
        {
            
        }
    }
}