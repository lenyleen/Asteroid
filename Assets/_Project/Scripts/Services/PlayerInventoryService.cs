using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Configs;
using _Project.Scripts.Data;
using _Project.Scripts.Installers;
using _Project.Scripts.Interfaces;
using Cysharp.Threading.Tasks;
using Unity.Services.Economy;

namespace _Project.Scripts.Services
{
    public class PlayerInventoryService : IPlayerInventoryService
    {
        private readonly UnityServicesInstaller  _unityServicesInstaller;
        private readonly IEconomyService _economyService;

        private bool _isAvailable;

        public PlayerInventoryService(UnityServicesInstaller unityServicesInstaller, IEconomyService economyService)
        {
            _unityServicesInstaller = unityServicesInstaller;
            _economyService = economyService;
        }

        public UniTask InitializeAsync()
        {
            _isAvailable = _unityServicesInstaller.Initialized && _unityServicesInstaller.Authenticated;

            return UniTask.CompletedTask;
        }

        public async UniTask<bool> HasItem(string id)
        {
            if (!_isAvailable)
                return false;

            var options = new GetInventoryOptions { InventoryItemIds = new List<string> { id } };

            var getResult = await _economyService.PlayerInventory
                .GetInventoryAsync(options)
                .AsUniTask();

            return getResult.PlayersInventoryItems.Any();
        }

        public async UniTask AddPurchaseReward(ProductConfig product,int amount)
        {
            if(!_isAvailable)
                return;

            switch (product.Type)
            {
                case PurchasingItemType.Currency:
                    await EconomyService.Instance.PlayerBalances.IncrementBalanceAsync(product.Id, amount);
                    break;
                case PurchasingItemType.Item:
                    await EconomyService.Instance.PlayerInventory.AddInventoryItemAsync(product.Id);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
