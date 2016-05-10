using System;
using System.Collections.Generic;

// ReSharper disable InconsistentNaming

namespace MagentoConnect.Models.Magento.CustomAttributes
{
    [Serializable]
    public class CustomAttributeSearchResource
    {
        public IEnumerable<CustomAttributeResource> items { get; set; }
        public int total_count { get; set; }
        public object search_criteria = null; //Omitted due to complexity
    }
}