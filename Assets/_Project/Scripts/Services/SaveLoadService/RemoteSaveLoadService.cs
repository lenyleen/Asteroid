using System;
using System.Collections.Generic;
using _Project.Scripts.Data;
using _Project.Scripts.Installers;
using _Project.Scripts.Interfaces;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Services.CloudSave;
using UnityEngine;

namespace _Project.Scripts.Services
{
    public class RemoteSaveLoadService : ISaveService, IProjectImportanceInitializable
    {
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

        public async UniTask SaveData<T>(T data, string key, DateTime timeOfCreation)  where T : class, ILoadedData
        {
            if (!IsAvailable)
            {
                Debug.LogWarning("RemoteSaveLoadService is not initialized");
                return;
            }

            var dataToSave = JsonConvert.SerializeObject(data);

            await CloudSaveService.Instance.Data.Player.SaveAsync(
                new Dictionary<string, object>{{key,dataToSave}})
                .AsUniTask();
        }

        public async UniTask<DataSaveGetResult<T>> TryLoadData<T>(string key) where T : class, ILoadedData
        {
            if(!IsAvailable)
                return new DataSaveGetResult<T>(false, null);

            try
            {
                var loadedData = await CloudSaveService.Instance.Data.Player.LoadAsync(
                        new HashSet<string> { key })
                    .AsUniTask();

                var progressDataJson = loadedData[key].Value.GetAsString();
                var progressData = JsonConvert.DeserializeObject<T>(progressDataJson);

                var savedData =  new SavedData<T>(progressData, (DateTime)loadedData[key].Modified);

                return new DataSaveGetResult<T>(true, savedData);
            }
            catch (Exception e)
            {
                Debug.LogWarning("Failed to load player data: " + e.Message);
                return new DataSaveGetResult<T>(false, null);
            }
        }
    }
}
