using System.Collections.Generic;
using System.Linq;
using System.Threading;
using _Project.Scripts.Configs;
using _Project.Scripts.Data;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Other;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;
using UnityEngine;
using UnityEngine.Purchasing;

namespace _Project.Scripts.Services
{
    public class PurchaseService : IBootstrapInitializable
    {
        private readonly UiService _popupService;
        private readonly CancellationTokenSource _cancellationTokenSource;

        private List<PurchaseConfig> _purchases;
        private StoreController _storeController;

        public PurchaseService(UiService popupService)
        {
            _popupService = popupService;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public async UniTask InitializeAsync()
        {
            await UnityServices.InitializeAsync();

            await EconomyService.Instance.Configuration.SyncConfigurationAsync();

            var products = EconomyService.Instance.Configuration.GetRealMoneyPurchases();

            var productsDefinitions = new List<ProductDefinition>();

            foreach (var product in products)
                productsDefinitions.Add(new ProductDefinition(product.Id, ProductType.Consumable));

            _storeController = UnityIAPServices.StoreController();

            await _storeController.Connect();

            _storeController.FetchProducts(productsDefinitions);

            _storeController.OnProductsFetched += StoreControllerOnOnProductsFetched;
            _storeController.OnProductsFetchFailed += StoreControllerOnOnProductsFetchFailed;
            _storeController.OnStoreDisconnected += StoreControllerOnOnStoreDisconnected;
        }

        public async UniTask<Order> Buy(string productId)
        {
            var listener = InitializeListener();

            var product = _storeController.GetProductById(productId);
            return await listener.BuyProduct(product, _cancellationTokenSource.Token);
        }

        public List<ProductConfig> GetPurchaseItems(string purchasingId)
        {
            var purchase =
                EconomyService.Instance.Configuration.GetRealMoneyPurchase(purchasingId);

            var items = new List<ProductConfig>();

            foreach (var purchaseReward in purchase.Rewards)
            {
                var configJson = purchaseReward.Item.GetReferencedConfigurationItem().CustomDataDeserializable
                    .GetAsString();

                var config = JsonConvert.DeserializeObject<ProductConfig>(configJson);
                items.Add(config);
            }
            return items;
        }

        public IEnumerable<PurchaseConfig> GetPurchasesByType(PurchasingType purchasingType) =>
            _purchases.Where(item => item.PurchasingType == purchasingType);

        private List<PurchaseConfig> DeserializePurchases(List<RealMoneyPurchaseDefinition> realMoneyPurchases)
        {
            var purchases = new List<PurchaseConfig>();

            foreach (var realMoneyPurchase in realMoneyPurchases)
            {
                var configJson = realMoneyPurchase.CustomDataDeserializable.GetAsString();
                var config = JsonConvert.DeserializeObject<PurchaseConfig>(configJson);

                purchases.Add(config);
            }
            return purchases;
        }

        private PurchasingServiceShopListener InitializeListener()
        {
            var listener = new PurchasingServiceShopListener(_storeController);
            listener.Initialize();
            return listener;
        }

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
    }
}
