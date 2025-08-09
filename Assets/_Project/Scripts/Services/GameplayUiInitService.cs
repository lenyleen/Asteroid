using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Services
{
    public class GameplayUiInitService
    {
        private readonly AssetProvider _assetProvider;
        private readonly Transform _uiParent;

        public async UniTask InitializeAsync(Transform uiParent)
        {
            /*_uiParent = uiParent;
            _assetProvider = new AssetProvider();

            await LoadUiAssetsAsync();*/

            //TODO: доделать ини юи в сцене игры
        }
    }
}
