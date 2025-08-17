using System;
using System.Collections;
using _Project.Scripts.Interfaces;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Project.Scripts.Services.AssetProvider
{
    public class ScenesAssetProvider : AssetProviderBase, IScenesAssetProvider
    {
        public T GetLoadedAsset<T>(string assetKey) where T : class
        {
            if (!_completedHandles.TryGetValue(assetKey, out var handle) &&
                handle.Status != AsyncOperationStatus.Succeeded)
                throw new Exception("Asset not loaded: " + assetKey);

            return handle.Result as T;
        }

        public override void Dispose()
        {
            foreach (var handle in _handles.Values)
            foreach (var asyncOperationHandle in handle)
                Addressables.Release(asyncOperationHandle);

            _completedHandles.Clear();
            _handles.Clear();
        }
    }
}





