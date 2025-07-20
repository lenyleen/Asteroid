using System;
using DataObjects;
using Interfaces;
using UnityEngine;

namespace Other
{
    public class KillBox : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.TryGetComponent<ICollisionReceiver>(out var receiver))
                receiver.Collide(ColliderType.KillBox, Int32.MaxValue);
        }
    }
}