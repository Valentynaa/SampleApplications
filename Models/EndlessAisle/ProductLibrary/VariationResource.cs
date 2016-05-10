using System;

namespace MagentoConnect.Models.EndlessAisle.ProductLibrary
{
    [Serializable]
    public class VariationResource : BaseRevisionResource
    {
        public bool IsArchived { get; set; }
    }
}