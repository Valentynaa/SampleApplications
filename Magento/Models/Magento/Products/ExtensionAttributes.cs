using System;
using System.Collections.Generic;

// ReSharper disable InconsistentNaming

namespace MagentoSync.Models.Magento.Products
{
    [Serializable]
    public class ExtensionAttributes
    {
        public List<ConfigurableProductOption> configurable_product_options { get; set; }
        public List<int> configurable_product_links { get; set; }
    }
}