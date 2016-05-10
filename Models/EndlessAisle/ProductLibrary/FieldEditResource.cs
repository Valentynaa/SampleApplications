using System;

namespace MagentoConnect.Models.EndlessAisle.ProductLibrary
{
    [Serializable]
    public class FieldEditResource : FieldResource
    {
        public bool ForceOverride { get; set; }
    }
}