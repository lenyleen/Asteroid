using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Advertisements;
using UnityEngine.Purchasing;

namespace _Project.Scripts.Other
{
    public class PurchasingServiceShopListener
    {
        private readonly StoreController  _storeController;
        private readonly UniTaskCompletionSource<Order> _buyOrderTcs;

        public PurchasingServiceShopListener(StoreController storeController)
        {
            _buyOrderTcs =  new UniTaskCompletionSource<Order>();
            _storeController = storeController;
        }

        public void Initialize()
        {
            _storeController.OnPurchasePending += StoreControllerOnOnPurchasePending;
            _storeController.OnPurchaseFailed += StoreControllerOnOnPurchaseFailed;
        }

        public UniTask<Order> BuyProduct(Product product, CancellationToken cts)
        {
            _storeController.PurchaseProduct(product);
            return _buyOrderTcs.Task.AttachExternalCancellation(cts);
        }

        /*public UniTask<Order> BuyProduct(string productId, CancellationToken cts)
        {
            _storeController.PurchaseProduct(productId);
            return _buyOrderTcs.Task.AttachExternalCancellation(cts);
        }*/

        private void StoreControllerOnOnPurchaseFailed(FailedOrder failedOrder)
        {
            _buyOrderTcs.TrySetException(new Exception(failedOrder.Details));
            Dispose();
        }

        private void StoreControllerOnOnPurchasePending(PendingOrder order)
        {
            _buyOrderTcs.TrySetResult(order);
            Dispose();
        }

        private void Dispose()
        {
            _storeController.OnPurchasePending -= StoreControllerOnOnPurchasePending;
            _storeController.OnPurchaseFailed -= StoreControllerOnOnPurchaseFailed;
        }
    }
}
