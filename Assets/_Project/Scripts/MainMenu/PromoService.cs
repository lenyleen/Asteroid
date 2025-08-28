using System.Collections.Generic;
using _Project.Scripts.Configs;
using _Project.Scripts.Data;
using _Project.Scripts.Factories;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Other;
using _Project.Scripts.UI;
using Cysharp.Threading.Tasks;
using UniRx;

namespace _Project.Scripts.MainMenu
{
    public class PromoService : ISceneInitializable
    {
        private readonly IPlayerInventoryService _playerInventoryService;
        private readonly PromoButtonFactory _promoButtonFactory;
        private readonly PromoPopUpProvider _promoPopUpProvider;
        private readonly IPurchaseService _purchaseService;
        private readonly CompositeDisposable _disposables = new();

        private bool _promoOpened;

        public PromoService(IPlayerInventoryService playerInventoryService, PromoButtonFactory promoButtonFactory,
            PromoPopUpProvider promoPopUpProvider, IPurchaseService purchaseService)
        {
            _playerInventoryService = playerInventoryService;
            _promoButtonFactory = promoButtonFactory;
            _promoPopUpProvider = promoPopUpProvider;
            _purchaseService = purchaseService;
        }

        public async UniTask InitializeAsync()
        {
            var promos = _purchaseService.GetPurchasesByType(PurchasingType.Promo);

            foreach (var config in promos)
            {
                var rewards = _purchaseService.GetPurchaseItems(config.Id);

                if (!await CheckPromoToRelevance(rewards))
                    continue;

                var promoButton = await _promoButtonFactory.Create(config);

                promoButton.OnSelected.Subscribe(_ =>
                        PromoButtonSelected(config, promoButton))
                    .AddTo(_disposables);
            }
        }

        private async void PromoButtonSelected(PurchaseConfig config, PromoButton button)
        {
            if(_promoOpened)
                return;
            _promoOpened = true;

            var popUp = await _promoPopUpProvider.Get(config);

            var buyResult = await popUp.ShowDialogAsync(false);

            if (buyResult != DialogResult.Yes)
            {
                popUp.Hide();
                _promoOpened = false;
                return;
            }

            var purchaseResult = await _purchaseService.Buy(config.Id);

            if(purchaseResult == PurchaseResult.Complete)
                button.Hide();

            popUp.Hide();
            _promoOpened = false;
        }

        private async UniTask<bool> CheckPromoToRelevance(IEnumerable<ProductConfig> products)
        {
            foreach (var promo in products)
                if (await _playerInventoryService.HasItem(promo.Id))
                    return false;

            return true;
        }
    }
}
