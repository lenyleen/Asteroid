using System.Collections.Generic;
using _Project.Scripts.Data;

namespace _Project.Scripts.Configs
{
    public class PurchaseConfig
    {
        public PurchasingType  PurchasingType { get; set; }
        public string Id { get; private set; }
        public string PromoName { get; private set; }
        public string PromoDescription { get; private set; }
        public string PromoImageAddress { get; private set; }
        public decimal Price { get; private set; }
        public decimal PromoPrice { get; private set; }
    }
}
