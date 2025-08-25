using System;
using _Project.Scripts.Installers;
using _Project.Scripts.Interfaces;
using Cysharp.Threading.Tasks;
using Firebase.RemoteConfig;
using Newtonsoft.Json;
using UnityEngine;

namespace _Project.Scripts.Services
{
    public class FirebaseRemoteConfigService : IRemoteConfigService
    {
        private readonly FirebaseInstaller  _firebaseInstaller;

        private FirebaseRemoteConfig _remoteConfig;

        public FirebaseRemoteConfigService(FirebaseInstaller firebaseInstaller)
        {
            _firebaseInstaller = firebaseInstaller;
        }

        public async UniTask InitializeAsync()
        {
            if(!_firebaseInstaller.IsInitialized)
                await _firebaseInstaller.InitializeAsync();

            _remoteConfig = FirebaseRemoteConfig.DefaultInstance;

            var rc = FirebaseRemoteConfig.DefaultInstance;
            Debug.Log($"LastFetchStatus: {rc.Info.LastFetchStatus}");
            Debug.Log($"FailureReason: {rc.Info.LastFetchFailureReason}");
            Debug.Log($"LastFetchTime: {rc.Info.FetchTime}");


            await rc.FetchAsync(TimeSpan.Zero).AsUniTask();

            await rc.ActivateAsync();
        }

        public T GetConfig<T>(string key)
        {
            var value = _remoteConfig.GetValue(key).StringValue;

            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}
