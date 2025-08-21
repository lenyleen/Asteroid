using _Project.Scripts.Data;

namespace _Project.Scripts.Configs
{
    public class PurchaseConfig
    {
        public PurchasingType PurchasingType { get; set; }
        public string Id { get; private set; }
        public string PromoName { get; private set; }
        public string PromoDescription { get; private set; }
        public string PromoImageAddress { get; private set; }
        public decimal Price { get; private set; }
        public decimal PromoPrice { get; private set; }

        public PurchaseConfig(PurchasingType purchasingType, string id, string promoName,
            string promoDescription, string promoImageAddress, decimal price, decimal promoPrice)
        {
            PurchasingType = purchasingType;
            Id = id;
            PromoName = promoName;
            PromoDescription = promoDescription;
            PromoImageAddress = promoImageAddress;
            Price = price;
            PromoPrice = promoPrice;
        }
    }
}
