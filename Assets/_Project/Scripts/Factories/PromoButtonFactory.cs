using _Project.Scripts.Configs;
using _Project.Scripts.Interfaces;
using _Project.Scripts.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.Factories
{
    public class PromoButtonFactory
    {
        private readonly IScenesAssetProvider _assetProvider;
        private readonly DiContainer _instantiator;
        private readonly Transform _buttonsParent;
        private readonly AssetReference _prefabReference;

        public PromoButtonFactory(IScenesAssetProvider assetProvider, DiContainer instantiator,
            Transform buttonsParent, AssetReference prefabReference)
        {
            _assetProvider = assetProvider;
            _instantiator = instantiator;
            _buttonsParent = buttonsParent;
            _prefabReference = prefabReference;
        }

        public async UniTask<PromoButton> Create(PurchaseConfig promoConfig)
        {
            var sprite = await _assetProvider.Load<Sprite>(promoConfig.PromoImageAddress);

            var prefab = await _assetProvider.Load<GameObject>(_prefabReference);

            var button = _instantiator.InstantiatePrefabForComponent<PromoButton>(prefab, _buttonsParent);

            button.Initialize(sprite, promoConfig.PromoDescription);

            return button;
        }
    }
}
