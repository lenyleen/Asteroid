using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Configs;
using _Project.Scripts.Data;
using _Project.Scripts.Installers;
using _Project.Scripts.Other;
using _Project.Scripts.UI.PopUps;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;
using UnityEngine;
using UnityEngine.Purchasing;

namespace _Project.Scripts.Services
{
    public class PurchaseService
    {
        public bool IsAvailable { get; private set; }

        private readonly UiService _uiService;
        private readonly PlayerInventoryService _playerInventoryService;
        private readonly UnityServicesInstaller _unityServicesInstaller;

        private IEnumerable<PurchaseConfig> _purchases;
        private StoreController _storeController;

        public PurchaseService(UiService uiService, PlayerInventoryService playerInventoryService,
            UnityServicesInstaller  unityServicesInstaller)
        {
            _uiService = uiService;
            _playerInventoryService = playerInventoryService;
            _unityServicesInstaller = unityServicesInstaller;
        }

        public async UniTask InitializeAsync()
        {
            IsAvailable = _unityServicesInstaller.Initialized && _unityServicesInstaller.Authenticated;

            if (!IsAvailable)
                return;

            await EconomyService.Instance.Configuration
                .SyncConfigurationAsync()
                .AsUniTask();

            var products = EconomyService.Instance.Configuration.GetRealMoneyPurchases();

            _purchases = DeserializePurchases(products);

            var productsDefinitions = new List<ProductDefinition>();

            foreach (var product in products)
                productsDefinitions.Add(new ProductDefinition(product.Id, ProductType.Consumable));

            _storeController = UnityIAPServices.StoreController();

            await _storeController
                .Connect()
                .AsUniTask();

            _storeController.OnProductsFetched += StoreControllerOnOnProductsFetched;
            _storeController.OnProductsFetchFailed += StoreControllerOnOnProductsFetchFailed;
            _storeController.OnStoreDisconnected += StoreControllerOnOnStoreDisconnected;

            _storeController.FetchProducts(productsDefinitions);
        }

        public async UniTask<PurchaseResult> Buy(string productId)
        {
            if (!IsAvailable)
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
                    await _uiService.ShowDialogAwaitable<ErrorPopUp, ErrorPopUpData>(new ErrorPopUpData("Pusrchase is canceled"));

                await errorPopUp.ShowDialogAsync(true);
                return PurchaseResult.Failed;
            }
        }

        public IEnumerable<ProductConfig> GetPurchaseItems(string purchasingId)
        {
            if (!IsAvailable)
                return new List<ProductConfig>();

            var purchase = EconomyService.Instance.Configuration.GetRealMoneyPurchase(purchasingId);

            return purchase.Rewards
                .Select(DeserializeProduct);
        }

        public IEnumerable<PurchaseConfig> GetPurchasesByType(PurchasingType purchasingType)
        {
            if (!IsAvailable)
                return new List<PurchaseConfig>();

            return _purchases.Where(item => item.PurchasingType == purchasingType);
        }

        private async UniTask ConfirmPurchase(PendingOrder order, string productId)
        {
            var purchase = EconomyService.Instance.Configuration.GetRealMoneyPurchase(productId);

            foreach (var purchaseReward in purchase.Rewards)
            {
                var config = DeserializeProduct(purchaseReward);
                await _playerInventoryService.AddPurchaseReward(config, purchaseReward.Amount);
            }

            _storeController.ConfirmPurchase(order);
        }

        private IEnumerable<PurchaseConfig> DeserializePurchases(List<RealMoneyPurchaseDefinition> realMoneyPurchases) =>
            realMoneyPurchases.Select(DeserializePurchase);

        private PurchaseConfig DeserializePurchase(RealMoneyPurchaseDefinition realMoneyPurchase)
        {
            var configJson = realMoneyPurchase.CustomDataDeserializable.GetAsString();
            var config = JsonConvert.DeserializeObject<PurchaseConfig>(configJson);
            return config;
        }

        private ProductConfig DeserializeProduct(PurchaseItemQuantity item)
        {
            var configJson = item.Item.GetReferencedConfigurationItem().CustomDataDeserializable
                .GetAsString();

            return JsonConvert.DeserializeObject<ProductConfig>(configJson);
        }

        private PurchasingServiceShopListener InitializeListener()
        {
            var listener = new PurchasingServiceShopListener(_storeController);
            listener.Initialize();
            return listener;
        }

        #region Callbacks

        private void StoreControllerOnOnProductsFetchFailed(ProductFetchFailed fetchFailed)
        {
            Debug.Log(fetchFailed);
        }

        private void StoreControllerOnOnProductsFetched(List<Product> products)
        {
            Debug.Log("StoreControllerOnOnProductsFetched");
        }

        private void StoreControllerOnOnStoreDisconnected(StoreConnectionFailureDescription failureDescription)
        {
            Debug.Log("PurchasingService StoreControllerOnOnStoreDisconnected");
        }

        #endregion

    }
}
