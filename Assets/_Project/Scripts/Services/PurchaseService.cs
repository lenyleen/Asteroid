using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Configs;
using _Project.Scripts.Data;
using _Project.Scripts.Installers;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Other;
using _Project.Scripts.Static;
using _Project.Scripts.UI.PopUps;
using Cysharp.Threading.Tasks;
using Unity.Services.Economy;
using UnityEngine.Purchasing;
using IPurchaseService = _Project.Scripts.Interfaces.IPurchaseService;

namespace _Project.Scripts.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPopUpService _popUpService;
        private readonly IPlayerInventoryService _playerInventoryService;
        private readonly UnityServicesInstaller _unityServicesInstaller;
        private readonly StoreControllerInstaller _storeControllerInstaller;

        private IEconomyService _economyService;
        private IEnumerable<PurchaseConfig> _purchases;
        private bool _isAvailable;
        private StoreController _storeController;

        public PurchaseService(IPopUpService popUpService, IPlayerInventoryService playerInventoryService,
            UnityServicesInstaller  unityServicesInstaller, StoreControllerInstaller storeControllerInstaller)
        {
            _popUpService = popUpService;
            _playerInventoryService = playerInventoryService;
            _unityServicesInstaller = unityServicesInstaller;
            _storeControllerInstaller = storeControllerInstaller;
        }

        public async UniTask InitializeAsync()
        {
            _isAvailable = _unityServicesInstaller.Initialized && _unityServicesInstaller.Authenticated;

            if (!_isAvailable)
                return;

            _economyService = _unityServicesInstaller.GetEconomyService();

            await _economyService.Configuration
                .SyncConfigurationAsync()
                .AsUniTask();

            var products = _economyService.Configuration.GetRealMoneyPurchases();

            _purchases = PurchaseHelper.DeserializePurchases(products);

            _storeController = await _storeControllerInstaller.InitializeStoreController(products);
        }

        public async UniTask<PurchaseResult> Buy(string productId)
        {
            if (!_isAvailable)
                return PurchaseResult.Failed;

            var listener = InitializeListener();

            var product = _storeController.GetProductById(productId);

            try
            {
                var order = await listener.BuyProduct(product);

                await ConfirmPurchase((PendingOrder)order, productId);

                return PurchaseResult.Complete;
            }
            catch (Exception)
            {
                var errorPopUp =
                    await _popUpService.ShowDialogAwaitable<ErrorPopUp, ErrorPopUpData>(new ErrorPopUpData("Pusrchase is canceled"));

                await errorPopUp.ShowDialogAsync(true);
                return PurchaseResult.Failed;
            }
        }

        public IEnumerable<ProductConfig> GetPurchaseItems(string purchasingId)
        {
            if (!_isAvailable)
                return new List<ProductConfig>();

            var purchase = _economyService.Configuration.GetRealMoneyPurchase(purchasingId);

            return purchase.Rewards
                .Select(PurchaseHelper.DeserializeProduct);
        }

        public IEnumerable<PurchaseConfig> GetPurchasesByType(PurchasingType purchasingType)
        {
            if (!_isAvailable)
                return new List<PurchaseConfig>();

            return _purchases.Where(item => item.PurchasingType == purchasingType);
        }

        private async UniTask ConfirmPurchase(PendingOrder order, string productId)
        {
            var purchase = _economyService.Configuration.GetRealMoneyPurchase(productId);

            foreach (var purchaseReward in purchase.Rewards)
            {
                var config = PurchaseHelper.DeserializeProduct(purchaseReward);
                await _playerInventoryService.AddPurchaseReward(config, purchaseReward.Amount);
            }

            _storeController.ConfirmPurchase(order);
        }

        private PurchasingServiceShopListener InitializeListener()
        {
            var listener = new PurchasingServiceShopListener(_storeController);
            listener.Initialize();
            return listener;
        }
    }
}
