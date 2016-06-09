using System;

namespace MagentoSync.Models.EndlessAisle.ProductLibrary.Projections
{
    [Serializable]
    public class ProductDetailAssetResource
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Uri { get; set; }
        public string Type { get; set; }
        public bool IsHidden { get; set; }
    }
}