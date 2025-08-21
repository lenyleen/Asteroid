using System.Collections.Generic;
using _Project.Scripts.Configs;
using _Project.Scripts.Data;
using _Project.Scripts.Extensions;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Other;
using _Project.Scripts.Services;
using _Project.Scripts.UI.PopUps;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Factories
{
    public class PromoPopUpProvider
    {
        private readonly IScenesAssetProvider  _assetProvider;
        private readonly UiService  _uiService;

        public PromoPopUpProvider(IScenesAssetProvider assetProvider, UiService uiService)
        {
            _assetProvider = assetProvider;
            _uiService = uiService;
        }

        public async UniTask<PromoByuPopUp> Get(PurchaseConfig purchaseConfig)
        {
            var sprite = await _assetProvider.Load<Sprite>(purchaseConfig.PromoImageAddress);

            var popUpData = new PromoPopUpData(purchaseConfig.PromoName, purchaseConfig.PromoDescription,
                purchaseConfig.PromoPrice.ToNextPrice(), purchaseConfig.Price.ToPreviousPrice(), sprite);

            var popUp =
                await _uiService.ShowDialogAwaitable<PromoByuPopUp, PromoPopUpData, DialogResult>(popUpData);

            return popUp;
        }
    }
}
