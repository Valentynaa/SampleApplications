using System;
using System.Collections.Generic;

namespace MagentoConnect.Models.EndlessAisle.ProductLibrary.Projections
{
    [Serializable]
    public class GetBulkProductDetailsCommand
    {
        public IEnumerable<string> Slugs { get; set; }
    }
}