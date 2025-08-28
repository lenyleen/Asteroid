using _Project.Scripts.Configs;
using _Project.Scripts.Data;
using _Project.Scripts.Extensions;
using _Project.Scripts.Interfaces;
using _Project.Scripts.UI.PopUps;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Factories
{
    public class PromoPopUpProvider
    {
        private readonly IScenesAssetProvider _assetProvider;
        private readonly IPopUpService _popUpService;

        public PromoPopUpProvider(IScenesAssetProvider assetProvider, IPopUpService popUpService)
        {
            _assetProvider = assetProvider;
            _popUpService = popUpService;
        }

        public async UniTask<PromoByuPopUp> Get(PurchaseConfig purchaseConfig)
        {
            var sprite = await _assetProvider.Load<Sprite>(purchaseConfig.PromoImageAddress);

            var popUpData = new PromoPopUpData(purchaseConfig.PromoName, purchaseConfig.PromoDescription,
                purchaseConfig.PromoPrice.ToNextPrice(), purchaseConfig.Price.ToPreviousPrice(), sprite);

            var popUp =
                await _popUpService.ShowDialogAwaitable<PromoByuPopUp, PromoPopUpData>(popUpData);

            return popUp;
        }
    }
}
