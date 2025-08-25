using _Project.Scripts.Data;

namespace _Project.Scripts.Configs
{
    public class ProductConfig
    {
        public PurchasingItemType Type { get; private set; }
        public string Id { get; private set; }

        public ProductConfig(string id, PurchasingItemType type)
        {
            Id = id;
            Type = type;
        }
    }
}
