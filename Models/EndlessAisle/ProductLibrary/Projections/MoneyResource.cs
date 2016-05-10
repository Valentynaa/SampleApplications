using System;

namespace MagentoConnect.Models.EndlessAisle.ProductLibrary.Projections
{
    [Serializable]
    public class MoneyResource
    {
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
    }
}