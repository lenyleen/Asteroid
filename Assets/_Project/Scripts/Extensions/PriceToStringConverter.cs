using System.Globalization;

namespace _Project.Scripts.Extensions
{
    public static class PriceToStringConverter
    {
        public static string ToPreviousPrice(this decimal price)
        {
            var priceAsString = price.ToString("C", CultureInfo.GetCultureInfo("en-US"));
            return $"<s>{priceAsString}</s>";
            //#B0B0B0
        }

        public static string ToNextPrice(this decimal price)
        {
            var priceAsString = price.ToString("C", CultureInfo.GetCultureInfo("en-US"));
            return $"<color #FFD700>{priceAsString}</color>";
            //#FFD700
        }
    }
}
