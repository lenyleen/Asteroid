namespace _Project.Scripts.Configs.PopUpsConfigs
{
    public class PurchasePopUpConfig
    {
        public string SpriteAddress { get; private set; }

        public string Description { get; private set; }

        public decimal Price { get; private set; }

        public string ProductId { get; private set; }

        public string Name { get; private set; }

        public int Quantity { get; private set; }

        public PurchasePopUpConfig(int quantity, string name, string productId, decimal price, string description, string spriteAddress)
        {
            Quantity = quantity;
            Name = name;
            ProductId = productId;
            Price = price;
            Description = description;
            SpriteAddress = spriteAddress;
        }
    }
}
