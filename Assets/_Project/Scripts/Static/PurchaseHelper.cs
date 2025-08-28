using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Configs;
using Newtonsoft.Json;
using Unity.Services.Economy.Model;

namespace _Project.Scripts.Static
{
    public static class PurchaseHelper
    {
        public static IEnumerable<PurchaseConfig> DeserializePurchases(List<RealMoneyPurchaseDefinition> realMoneyPurchases) =>
            realMoneyPurchases.Select(DeserializePurchase);

        public static PurchaseConfig DeserializePurchase(RealMoneyPurchaseDefinition realMoneyPurchase)
        {
            var configJson = realMoneyPurchase.CustomDataDeserializable.GetAsString();
            var config = JsonConvert.DeserializeObject<PurchaseConfig>(configJson);
            return config;
        }

        public static ProductConfig DeserializeProduct(PurchaseItemQuantity item)
        {
            var configJson = item.Item.GetReferencedConfigurationItem().CustomDataDeserializable
                .GetAsString();

            return JsonConvert.DeserializeObject<ProductConfig>(configJson);
        }
    }
}
