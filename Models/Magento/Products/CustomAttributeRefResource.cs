using System;

// ReSharper disable InconsistentNaming

namespace MagentoConnect.Models.Magento.Products
{
    [Serializable]
    public class CustomAttributeRefResource
    {
        public string attribute_code { get; set; }
        public object value { get; set; }
    }
}