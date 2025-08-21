using _Project.Scripts.Interfaces;
using _Project.Scripts.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.Factories
{
    public class WeaponUiDataDisplayerFactory : ISceneInitializable
    {
        private readonly IInstantiator _instantiator;
        private readonly AssetReference _prefabReference;
        private readonly IScenesAssetProvider _assetProvider;

        private WeaponUiDataDisplayer _prefab;

        public WeaponUiDataDisplayerFactory(AssetReference prefabReference, DiContainer instantiator,
            IScenesAssetProvider assetProvider)
        {
            _prefabReference = prefabReference;
            _instantiator = instantiator;
            _assetProvider = assetProvider;
        }

        public async UniTask InitializeAsync()
        {
            _prefab = (await _assetProvider.Load<GameObject>(_prefabReference)).GetComponent<WeaponUiDataDisplayer>();
        }

        public WeaponUiDataDisplayer Create(IWeaponInfoProvider provider)
        {
            var displayer = _instantiator.InstantiatePrefabForComponent<WeaponUiDataDisplayer>(_prefab);
            displayer.Initialize(provider);
            return displayer;
        }
    }
}
