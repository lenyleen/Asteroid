using System;
using System.Collections.Generic;
using _Project.Scripts.Data;
using _Project.Scripts.DTO;
using _Project.Scripts.Installers;
using _Project.Scripts.Interfaces;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using Unity.Services.Core;
using UnityEngine;

namespace _Project.Scripts.Services
{
    public class RemoteSaveLoadService : ISaveService, IBootstrapInitializable
    {
        private const string PlayerProgressKey = "PlayerData";

        public bool IsAvailable { get; private set; }

        private readonly UnityServicesInstaller _unityServicesInstaller;

        public RemoteSaveLoadService(UnityServicesInstaller unityServicesInstaller)
        {
            _unityServicesInstaller = unityServicesInstaller;
        }

        public UniTask InitializeAsync()
        {
            IsAvailable = _unityServicesInstaller.Initialized && _unityServicesInstaller.Authenticated;
            return UniTask.CompletedTask;
        }

        public async UniTask SaveData(PlayerProgress data, DateTime timeOfCreation)
        {
            if (!IsAvailable)
            {
                Debug.LogWarning("RemoteSaveLoadService is not initialized");
                return;
            }

            var dataToSave = JsonConvert.SerializeObject(data);

            await CloudSaveService.Instance.Data.Player.SaveAsync(
                new Dictionary<string, object>{{PlayerProgressKey,dataToSave}})
                .AsUniTask();
        }

        public async UniTask<PlayerProgressSaveGetResult> TryLoadData()
        {
            if(!IsAvailable)
                return new PlayerProgressSaveGetResult(false, null);

            try
            {
                var loadedData = await CloudSaveService.Instance.Data.Player.LoadAsync(
                        new HashSet<string> { PlayerProgressKey })
                    .AsUniTask();

                var progressDataJson = loadedData[PlayerProgressKey].Value.GetAsString();
                var progressData = JsonConvert.DeserializeObject<PlayerProgress>(progressDataJson);

                var savedData =  new SavedPlayerData(progressData, (DateTime)loadedData[PlayerProgressKey].Modified);

                return new PlayerProgressSaveGetResult(true, savedData);
            }
            catch (Exception e)
            {
                Debug.LogWarning("Failed to load player data: " + e.Message);
                return new PlayerProgressSaveGetResult(false, null);
            }
        }
    }
}
