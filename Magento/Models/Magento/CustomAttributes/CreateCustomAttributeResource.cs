using System;

// ReSharper disable InconsistentNaming

namespace MagentoSync.Models.Magento.CustomAttributes
{
    [Serializable]
    public class CreateCustomAttributeResource
    {
        public CustomAttributeResource attribute { get; set; }
    }
}
