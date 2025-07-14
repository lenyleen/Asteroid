using UnityEngine;

namespace Interfaces
{
    public interface IPositionMutator : IPositionProvider
    {
        public void SetPosition(Vector3 position);
    }
}