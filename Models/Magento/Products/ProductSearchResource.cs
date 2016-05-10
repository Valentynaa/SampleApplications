using System;
using System.Collections.Generic;

// ReSharper disable InconsistentNaming

namespace MagentoConnect.Models.Magento.Products
{
    [Serializable]
    public class ProductSearchResource
    {
        public IEnumerable<ProductResource> items { get; set; }
        public int total_count { get; set; }
        public object search_criteria = null; //Omitted due to complexity
    }
}