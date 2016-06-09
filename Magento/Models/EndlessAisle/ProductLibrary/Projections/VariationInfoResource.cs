using System;
using System.Collections.Generic;

namespace MagentoSync.Models.EndlessAisle.ProductLibrary.Projections
{
    [Serializable]
    public class VariationInfoResource
    {
        public int VariationId { get; set; }
        public string Slug { get; set; }
        public IList<VaryingFieldResource> Fields { get; set; }
    }
}