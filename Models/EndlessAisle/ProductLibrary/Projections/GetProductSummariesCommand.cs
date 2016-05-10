using System;
using System.Collections.Generic;

namespace MagentoConnect.Models.EndlessAisle.ProductLibrary.Projections
{
    [Serializable]
    public class GetProductSummariesCommand
    {
        public IEnumerable<string> Slugs { get; set; }
    }
}