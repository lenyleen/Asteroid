using System;
using Cysharp.Threading.Tasks;
using Interfaces;
using UnityEngine;

namespace Services
{
    public class SaveLoadService : ISaveService
    {
        public UniTask<string> TryLoadData<T>(string name, out T data) where T : class, ISavableData
        {
            data = JsonUtility.FromJson<T>(PlayerPrefs.GetString(name));

            return data == null ?
                UniTask.FromResult("Data not found or invalid format") : UniTask.FromResult("");
        }

        public UniTask<string> TrySaveData(string name, object data)
        {
            try
            {
                var json = JsonUtility.ToJson(data);
                PlayerPrefs.SetString(name, json);
                return UniTask.FromResult("");
            }
            catch (Exception e)
            {
                return UniTask.FromResult(e.Message);
            }
        }
    }
}
