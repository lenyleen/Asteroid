using _Project.Scripts.Interfaces;
using _Project.Scripts.UI.PopUps;
using UnityEngine;

namespace _Project.Scripts.Data
{
    public class PromoPopUpData : IPopUpParams<PromoByuPopUp>
    {
        public string ProductName { get; private set; }
        public string ProductDescription { get; private set; }
        public string ProductPrice { get; private set; }
        public string ProductPrevPrice { get; private set; }
        public Sprite ProductImage { get; private set; }

        public PromoPopUpData(string productName, string productDescription, string productPrice,
            string productPrevPrice, Sprite productImage)
        {
            ProductName = productName;
            ProductDescription = productDescription;
            ProductPrice = productPrice;
            ProductPrevPrice = productPrevPrice;
            ProductImage = productImage;
        }
    }
}
