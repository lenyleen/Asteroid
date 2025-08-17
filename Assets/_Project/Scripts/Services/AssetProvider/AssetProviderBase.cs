using System.Collections.Generic;
using _Project.Scripts.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Project.Scripts.Services.AssetProvider
{
    public abstract class AssetProviderBase : IAssetsProvider
    {
        protected Dictionary<string, AsyncOperationHandle> _completedHandles = new ();
        protected Dictionary<string, List<AsyncOperationHandle>> _handles = new();

        public async UniTask<T> Load<T>(AssetReference reference) where T : class
        {
            if (_completedHandles.TryGetValue(reference.AssetGUID, out var completedHandle))
                return completedHandle.Result as T;

            return await CompleteHandle(Addressables.LoadAssetAsync<T>(reference), reference.AssetGUID);
        }

        public async UniTask<T> Load<T>(string assetKey) where T : class
        {
            if(_completedHandles.TryGetValue(assetKey, out var completedHandle))
                return completedHandle.Result as T;

            return await CompleteHandle(Addressables.LoadAssetAsync<T>(assetKey), assetKey);
        }


        public abstract void Dispose();

        private async UniTask<T> CompleteHandle<T>(AsyncOperationHandle<T> handle, string referenceKey) where T : class
        {
            handle.Completed += hndl =>
                _completedHandles[referenceKey] = hndl;

            AddHandle(referenceKey, handle);
            return await handle.ToUniTask();
        }

        private void AddHandle<T>(string id, AsyncOperationHandle<T> handle) where T : class
        {
            if (!_handles.TryGetValue(id, out var handles))
            {
                handles = new List<AsyncOperationHandle>();
                _handles[id] = handles;
            }

            handles.Add(handle);
        }
    }
}
