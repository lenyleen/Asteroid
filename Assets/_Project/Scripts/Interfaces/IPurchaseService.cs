using System.Collections.Generic;
using _Project.Scripts.Configs;
using _Project.Scripts.Data;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Interfaces
{
    public interface IPurchaseService : IBootstrapInitializable
    {
        UniTask<PurchaseResult> Buy(string productId);
        IEnumerable<ProductConfig> GetPurchaseItems(string purchasingId);
        IEnumerable<PurchaseConfig> GetPurchasesByType(PurchasingType purchasingType);
    }
}
