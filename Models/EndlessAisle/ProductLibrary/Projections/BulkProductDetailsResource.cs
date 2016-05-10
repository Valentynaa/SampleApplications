using System;
using System.Collections.Generic;

namespace MagentoConnect.Models.EndlessAisle.ProductLibrary.Projections
{
    [Serializable]
    public class BulkProductDetailsResource
    {
        public IDictionary<string, ProductDetailsResource> Products { get; set; }
    }
}