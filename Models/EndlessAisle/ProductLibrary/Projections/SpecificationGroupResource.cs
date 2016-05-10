using System;
using System.Collections.Generic;

namespace MagentoConnect.Models.EndlessAisle.ProductLibrary.Projections
{
    [Serializable]
    public class SpecificationGroupResource
    {
        public string Name { get; set; }
        public List<SpecificationFieldResource> Fields { get; set; }
    }
}