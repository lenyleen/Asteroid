using System;
using Cysharp.Threading.Tasks;
using Interfaces;
using UnityEngine;

namespace Services
{
    public class SaveLoadService : ISaveService
    {
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

        public UniTask<string> TryLoadData<T>(string name, out T data) where T : class, ISavableData
        {
            try
            {
                data = JsonUtility.FromJson<T>(PlayerPrefs.GetString(name));
                return UniTask.FromResult("");
            }
            catch (Exception e)
            {
                data = null;
                return UniTask.FromResult(e.Message);
            }
        }
    }
}
