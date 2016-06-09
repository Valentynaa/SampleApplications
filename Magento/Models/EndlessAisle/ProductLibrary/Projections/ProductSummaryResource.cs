using System;

namespace MagentoSync.Models.EndlessAisle.ProductLibrary.Projections
{
    [Serializable]
    public class ProductSummaryResource
    {
        public string Id { get; set; }
        public int MasterProductId { get; set; }
        public int? VariationId { get; set; }
        public string Name { get; set; }

        public ProductClassificationRefResource Classification { get; set; }

        public string ShortDescription { get; set; }
        public EntityRefResource Manufacturer { get; set; }

        public Guid? HeroShotId { get; set; }
    }
}