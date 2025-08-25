using System;
using _Project.Scripts.Data;
using _Project.Scripts.DTO;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Interfaces
{
    public interface ISaveService
    {
        public UniTask SaveData(PlayerProgress data, DateTime timeOfCreation);

        public UniTask<PlayerProgressSaveGetResult> TryLoadData();
    }
}
