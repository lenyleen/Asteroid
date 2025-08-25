using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Project.Scripts.Configs;
using _Project.Scripts.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.Factories
{
    public class PopUpFactory : IBootstrapInitializable
    {
        private readonly PopUpsConfig _popUpsConfig;
        private readonly IProjectAssetProvider _assetProvider;
        private readonly Transform _popUpParent;
        private readonly IInstantiator _instantiator;

        private Dictionary<Type, AssetReferenceGameObject> _popUpPrefabs = new ();

        public PopUpFactory(PopUpsConfig config, Transform popUpParent,
            IInstantiator instantiator, IProjectAssetProvider assetProvider)
        {
            _popUpsConfig = config;
            _assetProvider = assetProvider;
            _popUpParent = popUpParent;
            _instantiator = instantiator;
        }

        public async UniTask InitializeAsync()
        {
            foreach (var reference in _popUpsConfig.PopUpPrefabsReferences)
            {
                var prefab = await _assetProvider.Load<GameObject>(reference);
                var componentGetResul = prefab.TryGetComponent<IPopUp>(out var component);

                if(!componentGetResul)
                {
                    Debug.LogWarning("PopUp prefab does not implement IPopUp interface: " + prefab.name);
                    continue;
                }

                _popUpPrefabs.TryAdd(component.GetType(),reference);

                _assetProvider.RemoveLoadedAsset(reference);
            }
        }

        public async Task<T> CreatePopUp<T>() where T : IPopUp
        {
            var type = typeof(T);
            if (!_popUpPrefabs.TryGetValue(type, out var reference))
                throw new Exception($"No prefab found for {type.Name}");

            var prefab = await _assetProvider.Load<GameObject>(reference);

            var instance = _instantiator.InstantiatePrefabForComponent<T>(prefab);
            instance.Initialize(_popUpParent);

            return instance;
        }
    }
}
