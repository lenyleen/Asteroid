using System;
using _Project.Scripts.Data;
using _Project.Scripts.Interfaces;
using Cysharp.Threading.Tasks;
using ModestTree;
using Newtonsoft.Json;
using UnityEngine;

namespace _Project.Scripts.Services
{
    public class LocalSaveLoadService : ISaveService
    {
        public UniTask SaveData<T>(T data,string key, DateTime dateOfCreation) where T : class, ILoadedData
        {
            var dataToSave = new SavedData<T>(data, dateOfCreation);

            var json = JsonConvert.SerializeObject(dataToSave);
            PlayerPrefs.SetString(key, json);
            return UniTask.CompletedTask;
        }

        public UniTask<DataSaveGetResult<T>> TryLoadData<T>(string key) where T : class,ILoadedData
        {
            try
            {
                var savedData = PlayerPrefs.GetString(key);

                if(savedData.IsEmpty())
                    return UniTask.FromResult(new DataSaveGetResult<T>(false, null));

                var data = JsonConvert.DeserializeObject<SavedData<T>>(savedData);

                return UniTask.FromResult(new DataSaveGetResult<T>(true, data));
            }
            catch (Exception e)
            {

                Debug.LogWarning("Failed to load player data: " + e.Message);
                return UniTask.FromResult(new DataSaveGetResult<T>(false, null));
            }
        }
    }

    [Serializable]
    public class SavedData<T> where T : ILoadedData
    {
        public T LoadedData { get; set; }
        public DateTime Created { get; set; }

        public SavedData(T loadedData, DateTime created)
        {
            LoadedData = loadedData;
            Created = created;
        }
    }
}
