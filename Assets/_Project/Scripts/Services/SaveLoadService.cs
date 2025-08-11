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

        public UniTask SaveData(PlayerProgress data)
        {
            var json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(_playerProgressKey, json);
            return UniTask.CompletedTask;
        }

        public UniTask<bool> TryLoadData(out PlayerProgress data)
        {
            data = JsonUtility.FromJson<PlayerProgress>(PlayerPrefs.GetString(_playerProgressKey));

            return UniTask.FromResult(data == null);
        }
    }
}
