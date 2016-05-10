using System;
using System.Collections.Generic;

// ReSharper disable InconsistentNaming

namespace MagentoConnect.Models.Magento.Products
{
    [Serializable]
    public class UpdateProductBodyResource
    {
        public int id { get; set; }
        public string sku { get; set; }
        public List<CustomAttributeRefResource> custom_attributes { get; set; }
    }
}