using System;

namespace MagentoSync.Models.EndlessAisle.ProductLibrary
{
    [Serializable]
    public class ProductClassificationRefResource
    {
        public int TreeId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}