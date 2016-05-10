using System;
using System.Collections.Generic;

namespace MagentoConnect.Models.EndlessAisle.ProductLibrary
{
    [Serializable]
    public class ProductIdentifierGroupResource
    {
        public string Type { get; set; }
        public List<ProductIdentifierResource> Identifiers { get; set; }

        public bool ForceOverride { get; set; }

        public ProductIdentifierGroupResource()
        {
            Identifiers = new List<ProductIdentifierResource>();
        }
    }
}
