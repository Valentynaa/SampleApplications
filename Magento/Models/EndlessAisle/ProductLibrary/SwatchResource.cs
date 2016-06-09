using System;

namespace MagentoSync.Models.EndlessAisle.ProductLibrary
{
    [Serializable]
    public class SwatchResource
    {
        public string Type { get; set; }
        public Guid? AssetId { get; set; }
        public string ColorCode { get; set; }
    }
}