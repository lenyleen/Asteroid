using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Services
{
    public class AssetProvider
    {
        Dictionary<string, AsyncOperationHandle> _completedHandles = new ();
        Dictionary<string, List<AsyncOperationHandle>> _handles = new();

        public async Task<T> Load<T>(AssetReference reference) where T : class
        {
            if (_completedHandles.TryGetValue(reference.AssetGUID, out var completedHandle))
                return completedHandle.Result as T;

            return await CompleteHandle(Addressables.LoadAssetAsync<T>(reference), reference.AssetGUID);
        }

        private async UniTask<T> CompleteHandle<T>(AsyncOperationHandle<T> handle, string referenceKey) where T : class
        {
            handle.Completed += hndl =>
                _completedHandles[referenceKey] = hndl;

            AddHandle(referenceKey, handle);
            return await handle.Task;
        }

        public void ReleasePrefab(string assetGUID)
        {
            foreach (var handle in _handles[assetGUID])
                handle.Release();

            _completedHandles.Remove(assetGUID);
        }

        public void Dispose()
        {
            foreach (var handle in _handles.Values)
                foreach (var asyncOperationHandle in handle)
                    Addressables.Release(asyncOperationHandle);

            _completedHandles.Clear();
            _handles.Clear();
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



/*public async Task<T> Load<T>(string assetKey) where T : class
        {
            if(_completedHandles.TryGetValue(assetKey, out var completedHandle))
                return completedHandle.Result as T;

            return await CompleteHandle(Addressables.LoadAssetAsync<T>(assetKey), assetKey);
        }*/
