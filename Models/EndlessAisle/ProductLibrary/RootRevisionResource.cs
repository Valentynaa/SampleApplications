using System;
using System.Collections.Generic;

namespace MagentoConnect.Models.EndlessAisle.ProductLibrary
{
    [Serializable]
    public class RootRevisionResource : BaseRevisionResource
    {
        public RootRevisionResource()
        {
            Variations = new List<VariationResource>();
        }

        public List<VariationResource> Variations { get; set; }

        public bool IsArchived { get; set; }
    }
}
