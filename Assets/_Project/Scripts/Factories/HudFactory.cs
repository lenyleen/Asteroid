using _Project.Scripts.Interfaces;
using _Project.Scripts.UI;
using _Project.Scripts.UI.ScoreBox;
using _Project.Scripts.UI.ShipInfoInfo;
using _Project.Scripts.UI.Tutorial;
using _Project.Scripts.UI.WeaponUi;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.Factories
{
    public class HudFactory : ISceneInitializable
    {
        private readonly IScenesAssetProvider _assetProvider;
        private readonly AssetReference _hudPrefabReference;
        private readonly DiContainer _container;
        private readonly Transform _hudParent;

        public HudFactory(IScenesAssetProvider assetProvider, AssetReference hudPrefabReference, DiContainer container,
            Transform hudParent)
        {
            _hudPrefabReference = hudPrefabReference;
            _assetProvider = assetProvider;
            _container = container;
            _hudParent = hudParent;
        }

        public async UniTask InitializeAsync()
        {
            var hudPrefab = await _assetProvider.Load<GameObject>(_hudPrefabReference);

            var hud = _container.InstantiatePrefabForComponent<HUD>(hudPrefab, _hudParent);

            _container.Bind<WeaponUiDisplayerView>()
                .FromComponentInNewPrefab(hud.WeaponUiDisplayerView)
                .AsSingle();

            _container.Bind<ScoreBoxView>()
                .FromComponentInNewPrefab(hud.ScoreBoxView)
                .AsSingle();

            _container.Bind<ShipInfoView>()
                .FromComponentInNewPrefab(hud.ShipInfoView)
                .AsSingle();

            _container.Bind<TutorialView>()
                .FromComponentInNewPrefab(hud.TutorialView)
                .AsSingle();
        }
    }
}
