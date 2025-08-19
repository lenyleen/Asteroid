using System;
using System.Collections.Generic;
using System.Threading;
using _Project.Scripts.Data;
using _Project.Scripts.Interfaces;
using _Project.Scripts.Other;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Purchasing;

namespace _Project.Scripts.Services
{
    public class PurchasingService : IBootstrapInitializable
    {
        private readonly PopupService _popupService;
        private readonly CancellationTokenSource _cancellationTokenSource;

        private StoreController _storeController;

        public PurchasingService(PopupService popupService)
        {
            _popupService = popupService;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public async UniTask InitializeAsync()
        {
            _storeController = UnityIAPServices.StoreController();

            await _storeController.Connect();

            _storeController.OnStoreDisconnected += StoreControllerOnOnStoreDisconnected;
        }

        public async UniTask<Order> Buy(string productId)
        {
            var listener = InitializeListener();

            var product = _storeController.GetProductById(productId);
            return await listener.BuyProduct(product, _cancellationTokenSource.Token);
        }
        private PurchasingServiceShopListener InitializeListener()
        {
            var listener = new PurchasingServiceShopListener(_storeController);
            listener.Initialize();
            return listener;
        }

        private void StoreControllerOnOnStoreDisconnected(StoreConnectionFailureDescription failureDescription)
        {
            Debug.Log("PurchasingService StoreControllerOnOnStoreDisconnected");
        }
    }
}
