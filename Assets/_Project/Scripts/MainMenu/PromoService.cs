using System.Collections.Generic;
using _Project.Scripts.Configs;
using _Project.Scripts.Data;
using _Project.Scripts.Factories;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Other;
using _Project.Scripts.Services;
using Cysharp.Threading.Tasks;
using UniRx;

namespace _Project.Scripts.MainMenu
{
    public class PromoService : ISceneInitializable
    {
        private readonly PlayerInventoryService _playerInventoryService;
        private readonly PromoButtonFactory _promoButtonFactory;
        private readonly PromoPopUpProvider _promoPopUpProvider;
        private readonly PurchaseService _purchaseService;
        private readonly CompositeDisposable _disposables = new();

        public async UniTask InitializeAsync()
        {
            var promos = _purchaseService.GetPurchasesByType(PurchasingType.Promo);

            foreach (var config in promos)
            {
                var promoButton = await _promoButtonFactory.Create(config);

                var rewards = _purchaseService.GetPurchaseItems(config.Id);

                var isRelevant = await CheckPromoToRelevance(rewards);

                if (isRelevant)
                    continue;

                promoButton.OnSelected.Subscribe(_ =>
                        PromoButtonSelected(config))
                    .AddTo(_disposables);
            }
        }

        private async void PromoButtonSelected(PurchaseConfig config)
        {
            var popUp = await _promoPopUpProvider.Get(config);

            var buyResult = await popUp.ShowDialogAsync();

            if (buyResult != DialogResult.Yes)
                return;

            await _purchaseService.Buy(config.Id);
        }

        private async UniTask<bool> CheckPromoToRelevance(List<ProductConfig> products)
        {
            foreach (var promo in products)
                if (await _playerInventoryService.HasItem(promo.Id))
                    return false;

            return true;
        }
    }
}
