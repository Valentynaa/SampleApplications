using System;
using System.Collections.Generic;

// ReSharper disable InconsistentNaming

namespace MagentoSync.Models.Magento.Products
{
    [Serializable]
    public class ConfigurableProductOption
    {
        public int id { get; set; }
        public string attribute_id { get; set; }
        public string label { get; set; }
        public int position { get; set; }
        public List<ConfigurableValue> values { get; set; }
        public int product_id { get; set; }
    }
}