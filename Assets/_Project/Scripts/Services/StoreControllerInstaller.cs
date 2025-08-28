using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.Services.Economy.Model;
using UnityEngine;
using UnityEngine.Purchasing;

namespace _Project.Scripts.Services
{
    public class StoreControllerInstaller
    {
        public async UniTask<StoreController> InitializeStoreController(List<RealMoneyPurchaseDefinition> products)
        {
            var productsDefinitions = new List<ProductDefinition>();

            foreach (var product in products)
                productsDefinitions.Add(new ProductDefinition(product.Id, ProductType.Consumable));

            var storeController = UnityIAPServices.StoreController();

            await storeController
                .Connect()
                .AsUniTask();

            storeController.OnProductsFetched += StoreControllerOnOnProductsFetched;
            storeController.OnProductsFetchFailed += StoreControllerOnOnProductsFetchFailed;
            storeController.OnStoreDisconnected += StoreControllerOnOnStoreDisconnected;

            storeController.FetchProducts(productsDefinitions);

            return storeController;
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
