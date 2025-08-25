using System.Collections;
using _Project.Scripts.Interfaces;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.Services.AssetProvider
{
    public class ProjectAssetProvider : AssetProviderBase, IProjectAssetProvider
    {
        public void RemoveLoadedAsset(AssetReference reference) =>
            RemoveLoadedAsset(reference.AssetGUID);

        public override void Dispose()
        {
            foreach (var handle in _handles.Values)
            foreach (var asyncOperationHandle in handle)
                Addressables.Release(asyncOperationHandle);

            _completedHandles = null;
            _handles = null;
        }

        private void RemoveLoadedAsset(string assetKey)
        {
            RemoveAsset(assetKey, _completedHandles);
            RemoveAsset(assetKey, _handles);;
        }

        private void RemoveAsset(string assetKey, IDictionary handleDictionary)
        {
            if(!handleDictionary.Contains(assetKey))
                return;

            handleDictionary.Remove(assetKey);
        }
    }
}
