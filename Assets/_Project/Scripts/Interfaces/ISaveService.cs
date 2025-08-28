using System;
using _Project.Scripts.Data;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Interfaces
{
    public interface ISaveService
    {
        public UniTask SaveData<T>(T data,string key, DateTime timeOfCreation) where T : class, ILoadedData;

        public UniTask<DataSaveGetResult<T>> TryLoadData<T>(string key) where T : class,ILoadedData;
    }
}
