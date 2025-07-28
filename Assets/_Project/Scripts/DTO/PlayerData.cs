using System;
using Interfaces;

namespace _Project.Scripts.DTO
{
    [Serializable]
    public class PlayerData : ISavableData
    {
        public int Score { get; set; }
    }
}
