using Interfaces;
using UnityEngine;

namespace DataObjects
{
    public class FireData
    {
        public IProjectileBehaviour Behaviour { get; private set; }
        public FireData(IProjectileBehaviour behaviour)
        {
            Behaviour = behaviour;
        }
    }
}