using System;
using System.Collections.Generic;

namespace MagentoConnect.Models.EndlessAisle.ProductLibrary
{
    [Serializable]
    public class ProductRevisionResource : BaseRevisionResource
    {
        public int Id { get; set; }
        
        //public int? ParentId { get; set; }

        public EntityRefResource Entity { get; set; }
        public HashSet<RegionRefResource> Regions { get; set; }
    }
}
