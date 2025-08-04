using System;
using _Project.Scripts.DTO;
using _Project.Scripts.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Services
{
    public class SaveLoadService : ISaveService
    {
        private const string _playerProgressKey = "PlayerData";

        public UniTask<string> TrySaveData(object data)
        {
            try
            {
                var json = JsonUtility.ToJson(data);
                PlayerPrefs.SetString(_playerProgressKey, json);
                return UniTask.FromResult("");
            }
            catch (Exception e)
            {
                return UniTask.FromResult(e.Message);
            }
        }

        public UniTask<string> TryLoadData(out PlayerProgress data)
        {
            data = JsonUtility.FromJson<PlayerProgress>(PlayerPrefs.GetString(_playerProgressKey));

            return data == null ? UniTask.FromResult("Data not found or invalid format") : UniTask.FromResult("");
        }
    }
}
