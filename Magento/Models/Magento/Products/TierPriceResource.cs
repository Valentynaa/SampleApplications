using System;

// ReSharper disable InconsistentNaming

namespace MagentoSync.Models.Magento.Products
{
    [Serializable]
    public class TierPriceResource
    {
        public int customer_group_id { get; set; }
        public  int qty { get; set; }
        public int value { get; set; }
        public object extension_attributes = null; //This property is not covered
    }
}