using System;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.Interfaces
{
    public interface IAssetsProvider
    {
        public UniTask<T> Load<T>(AssetReference reference) where T : class;
        public UniTask<T> Load<T>(string assetKey) where T : class;
        public void Dispose();
    }

    public interface IProjectAssetProvider : IAssetsProvider, IDisposable
    {
        public void RemoveLoadedAsset(AssetReference reference);
    }

    public interface IScenesAssetProvider : IAssetsProvider
    {
        public T GetLoadedAsset<T>(string assetKey) where T : class;
    }
}
