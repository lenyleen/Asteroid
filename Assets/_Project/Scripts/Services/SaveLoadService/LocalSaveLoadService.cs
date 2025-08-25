using System;
using _Project.Scripts.Data;
using _Project.Scripts.DTO;
using _Project.Scripts.Interfaces;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace _Project.Scripts.Services
{
    public class LocalSaveLoadService : ISaveService
    {
        private const string PlayerProgressKey = "PlayerData";

        public UniTask SaveData(PlayerProgress data, DateTime timeOfCreation)
        {
            var dataToSave = new SavedPlayerData(data, DateTime.UtcNow);

            var json = JsonConvert.SerializeObject(dataToSave);
            PlayerPrefs.SetString(PlayerProgressKey, json);
            return UniTask.CompletedTask;
        }

        public UniTask<PlayerProgressSaveGetResult> TryLoadData()
        {
            try
            {
                var data = JsonConvert.DeserializeObject<SavedPlayerData>(PlayerPrefs.GetString(PlayerProgressKey));

                return UniTask.FromResult(new PlayerProgressSaveGetResult(true, data));
            }
            catch (Exception e)
            {

                Debug.LogWarning("Failed to load player data: " + e.Message);
                return UniTask.FromResult(new PlayerProgressSaveGetResult(false, null));
            }
        }
    }

    [Serializable]
    public class SavedPlayerData
    {
        public PlayerProgress PlayerData { get; set; }
        public DateTime Created { get; set; }

        public SavedPlayerData(PlayerProgress playerData, DateTime created)
        {
            PlayerData = playerData;
            Created = created;
        }
    }
}
