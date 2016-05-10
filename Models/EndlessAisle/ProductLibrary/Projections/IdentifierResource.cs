using System;

namespace MagentoConnect.Models.EndlessAisle.ProductLibrary.Projections
{
    [Serializable]
    public class IdentifierResource
    {
        public string Value { get; set; }
        public string Description { get; set; }
        public EntityRefResource Entity { get; set; }
    }
}