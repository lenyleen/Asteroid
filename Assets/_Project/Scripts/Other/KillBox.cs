using _Project.Scripts.Configs;
using _Project.Scripts.Interfaces;
using UnityEngine;

namespace _Project.Scripts.Other
{
    public class KillBox : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<ICollisionReceiver>(out var receiver))
                receiver.Collide(ColliderType.KillBox, int.MaxValue);
        }
    }
}
