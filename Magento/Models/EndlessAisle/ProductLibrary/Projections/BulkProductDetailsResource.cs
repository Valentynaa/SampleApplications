using System;
using System.Collections.Generic;

namespace MagentoSync.Models.EndlessAisle.ProductLibrary.Projections
{
    [Serializable]
    public class BulkProductDetailsResource
    {
        public IDictionary<string, ProductDetailsResource> Products { get; set; }
    }
}