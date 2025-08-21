using _Project.Scripts.Data;

namespace _Project.Scripts.Configs
{
    public class ProductConfig
    {
        public InventoryItemType ItemType { get;  private set; }
        public string Name { get; private set; }

        public string Id { get; private set; }

        public string ImageAddress { get; private set; }

        public string Description { get; private set; }

        public decimal Price { get; private set; }

        public int Quantity { get; private set; }

        public ProductConfig(string name, string description, decimal price, int quantity,
            string imageAddress, InventoryItemType itemType, string id)
        {
            Name = name;
            Description = description;
            Price = price;
            Quantity = quantity;
            ImageAddress = imageAddress;
            ItemType = itemType;
            Id = id;
        }
    }
}
