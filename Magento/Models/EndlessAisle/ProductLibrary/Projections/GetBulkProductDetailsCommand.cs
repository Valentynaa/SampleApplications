using System;
using System.Collections.Generic;

namespace MagentoSync.Models.EndlessAisle.ProductLibrary.Projections
{
    [Serializable]
    public class GetBulkProductDetailsCommand
    {
        public IEnumerable<string> Slugs { get; set; }
    }
}