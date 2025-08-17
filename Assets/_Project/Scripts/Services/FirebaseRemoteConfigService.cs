using System;
using _Project.Scripts.Installers;
using _Project.Scripts.Interfaces;
using Cysharp.Threading.Tasks;
using Firebase.RemoteConfig;
using Newtonsoft.Json;

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

            await _remoteConfig.FetchAndActivateAsync().AsUniTask();
        }

        public T GetConfig<T>(string key)
        {
            var value = _remoteConfig.GetValue(key).StringValue;

            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}
