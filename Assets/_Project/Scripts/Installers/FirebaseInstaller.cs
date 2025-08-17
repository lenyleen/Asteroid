using System;
using _Project.Scripts.Interfaces;
using Cysharp.Threading.Tasks;
using Firebase;
using UnityEngine;

namespace _Project.Scripts.Installers
{
    public class FirebaseInstaller : IAsyncInitializable
    {
        public bool IsInitialized { get; private set; }

        public async UniTask InitializeAsync()
        {
            var staus = await FirebaseApp.CheckAndFixDependenciesAsync().AsUniTask();

            if (staus != DependencyStatus.Available)
                throw new Exception("Couldn't resolve all Firebase dependencies");


            IsInitialized = true;
            Debug.Log("Firebase is ready");
        }
    }
}
