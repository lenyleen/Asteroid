using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Interfaces;
using Cysharp.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Economy;

namespace _Project.Scripts.Services
{
    public class PlayerInventoryService : IBootstrapInitializable
    {
        public async UniTask InitializeAsync()
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        public async UniTask<bool> HasItem(string id)
        {
            var options = new GetInventoryOptions(){ InventoryItemIds = new List<string>{id}};

            var getResult =  await EconomyService.Instance.PlayerInventory.GetInventoryAsync(options);

            return getResult.PlayersInventoryItems.Any();
        }

        public async UniTask Additem(string id) =>
            await EconomyService.Instance.PlayerInventory.AddInventoryItemAsync(id);
    }
}
