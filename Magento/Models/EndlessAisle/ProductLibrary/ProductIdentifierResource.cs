using System;

namespace MagentoSync.Models.EndlessAisle.ProductLibrary
{
    [Serializable]
    public class ProductIdentifierResource
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public EntityRefResource Entity { get; set; }
    }
}
