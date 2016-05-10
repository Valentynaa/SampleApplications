using System;

// ReSharper disable InconsistentNaming

namespace MagentoConnect.Models.Magento.Products
{
    [Serializable]
    public class ProductLinkResource
    {
        public string sku { get; set; }
        public string link_type { get; set; }
        public string linked_product_sku { get; set; }
        public string linked_product_type { get; set; }
        public int? position { get; set; }
        public object extension_attributes = null; //This property is not covered as it is very deep
    }
}