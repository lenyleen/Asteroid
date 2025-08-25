using _Project.Scripts.Interfaces;
using Cysharp.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace _Project.Scripts.Installers
{
    public class UnityServicesInstaller : IProjectImportanceInitializable
    {
        public bool Initialized { get; private set; }
        public bool Authenticated { get; private set; }

        public async UniTask InitializeAsync()
        {
            try
            {
                await UnityServices.InitializeAsync()
                    .AsUniTask();

                Initialized = true;
            }
            catch (RequestFailedException e)
            {
                Debug.LogWarning($"UnityServices not initialized, ErrorCode {e.ErrorCode}, Message {e.Message}");
            }

            try
            {
                await AuthenticationService.Instance
                    .SignInAnonymouslyAsync()
                    .AsUniTask();

                Authenticated = true;
            }
            catch (RequestFailedException e)
            {
                Debug.LogWarning($"Not authenticated, ErrorCode {e.ErrorCode}, Message {e.Message}");
                throw;
            }
        }
    }
}
