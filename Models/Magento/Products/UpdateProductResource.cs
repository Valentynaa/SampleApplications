using System;
using System.Collections.Generic;

// ReSharper disable InconsistentNaming

namespace MagentoConnect.Models.Magento.Products
{
    [Serializable]
    public class UpdateProductResource
    {
        public UpdateProductBodyResource product { get; set; }
    }
}