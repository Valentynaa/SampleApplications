using System;

namespace MagentoSync.Models.EndlessAisle.ProductLibrary
{
    [Serializable]
    public class FieldEditResource : FieldResource
    {
        public bool ForceOverride { get; set; }
    }
}