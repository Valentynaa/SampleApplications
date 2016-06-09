using System;
using System.Collections.Generic;

// ReSharper disable InconsistentNaming

namespace MagentoSync.Models.Magento.Products
{
    [Serializable]
    public class UpdateProductResource
    {
        public UpdateProductBodyResource product { get; set; }
    }
}