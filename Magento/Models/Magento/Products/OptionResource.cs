using System;

// ReSharper disable InconsistentNaming

namespace MagentoSync.Models.Magento.Products
{
    [Serializable]
    public class OptionResource
    {
        public string product_sku { get; set; }
        public int option_id { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public int sort_order { get; set; }
        public bool is_require { get; set; }
        public decimal price { get; set; }
        public string price_type { get; set; }
        public int max_characters { get; set; }
    }
}