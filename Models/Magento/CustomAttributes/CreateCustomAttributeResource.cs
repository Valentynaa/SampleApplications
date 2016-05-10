using System;

// ReSharper disable InconsistentNaming

namespace MagentoConnect.Models.Magento.CustomAttributes
{
    [Serializable]
    public class CreateCustomAttributeResource
    {
        public CustomAttributeResource attribute { get; set; }
    }
}
