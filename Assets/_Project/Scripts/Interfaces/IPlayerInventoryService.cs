using _Project.Scripts.Configs;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Interfaces
{
    public interface IPlayerInventoryService : IBootstrapInitializable
    {
        UniTask<bool> HasItem(string id);
        UniTask AddPurchaseReward(ProductConfig product,int amount);
    }
}
