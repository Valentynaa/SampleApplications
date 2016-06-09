using System;
using System.Collections;
using System.Collections.Generic;

namespace MagentoSync.Models.EndlessAisle.ProductLibrary
{
    [Serializable]
    public class RevisionGroupResource
    {
        public int Order { get; set; }
        public int? VariationId { get; set; }
        public RevisionGroupTypeResource GroupType { get; set; }
        public List<ProductRevisionResource> Revisions { get; set; }
    }
}
