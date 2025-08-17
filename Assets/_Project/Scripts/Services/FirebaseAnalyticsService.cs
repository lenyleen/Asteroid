using System.Collections.Generic;
using _Project.Scripts.Configs;
using _Project.Scripts.Enemies;
using _Project.Scripts.Installers;
using _Project.Scripts.Interfaces;
using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Analytics;
using UnityEngine;

namespace _Project.Scripts.Services
{
    public class FirebaseAnalyticsService : IAnalyticsService
    {
        private readonly Dictionary<string, int> _shotBullets = new();
        private readonly Dictionary<EnemyType, int> _killedEnemies = new();
        private readonly FirebaseInstaller _firebaseInstaller;

        private bool _isReady => _firebaseInstaller.IsInitialized;

        public FirebaseAnalyticsService(FirebaseInstaller firebaseInstaller)
        {
            _firebaseInstaller = firebaseInstaller;
        }

        public async UniTask InitializeAsync()
        {
            if(!_firebaseInstaller.IsInitialized)
                await _firebaseInstaller.InitializeAsync();
        }

        public void SendStartGameAnalytics()
        {
            if (!_isReady)
                return;

            FirebaseAnalytics.LogEvent("GameStart");
            Debug.Log("GameStartEventSend");
        }

        public void SendEndGameAnalytics()
        {
            if (!_isReady)
                return;

            var parameters = new List<Parameter>();

            ConvertDictionaryToParameters(_shotBullets, parameters);
            ConvertDictionaryToParameters(_killedEnemies, parameters);

            FirebaseAnalytics.LogEvent("EndGameData", parameters.ToArray());
            Debug.Log("EndGameEventSend");

            _killedEnemies.Clear();
            _shotBullets.Clear();
        }

        public void WeaponFire(WeaponType type, string weaponName)
        {
            if (type == WeaponType.Secondary && _isReady)
                FirebaseAnalytics.LogEvent($"Used {type}");

            IncrementCountOf(weaponName, _shotBullets);
        }

        public void EnemyKilled(EnemyType type)
        {
            IncrementCountOf(type, _killedEnemies);
        }

        private void ConvertDictionaryToParameters<T>(Dictionary<T, int> data, in List<Parameter> parameters)
        {
            foreach (var kvp in data)
                parameters.Add(new Parameter(kvp.Key.ToString(), kvp.Value));
        }

        private void IncrementCountOf<T>(T key, Dictionary<T, int> data)
        {
            if (data.TryAdd(key, 1))
                return;

            data[key]++;
        }
    }
}
