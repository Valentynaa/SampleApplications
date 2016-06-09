using System;

namespace MagentoSync.Models.EndlessAisle.ProductLibrary.Projections
{
    [Serializable]
    public class MoneyResource
    {
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
    }
}