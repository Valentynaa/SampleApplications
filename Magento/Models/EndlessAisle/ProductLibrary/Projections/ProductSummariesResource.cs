using System;
using System.Collections.Generic;

namespace MagentoSync.Models.EndlessAisle.ProductLibrary.Projections
{
    [Serializable]
    public class ProductSummariesResource
    {
        public IDictionary<string, ProductSummaryResource> Summaries { get; set; }
    }
}