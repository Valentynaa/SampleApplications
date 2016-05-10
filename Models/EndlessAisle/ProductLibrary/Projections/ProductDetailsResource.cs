using System;
using System.Collections.Generic;

namespace MagentoConnect.Models.EndlessAisle.ProductLibrary.Projections
{
    [Serializable]
    public class ProductDetailsResource 
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int MasterProductId { get; set; }
        public int? VariationId { get; set; }

        public EntityRefResource Owner { get; set; }

        public ClassificationBreadcrumbResource CanonicalClassification { get; set; }

        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public EntityRefResource Manufacturer { get; set; }
        public MoneyResource MSRP { get; set; }
        public DateTime? ReleaseDate { get; set; }

        public IEnumerable<VariationInfoResource> VariationInfo { get; set; }
        public IEnumerable<SpecificationGroupResource> Specifications { get; set; }
        public IEnumerable<ProductDetailAssetResource> Assets { get; set; }

        public ColorDefinitionResource ColorDefinition { get; set; }

        public string HeroShotUri { get; set; }
        public Guid? HeroShotId { get; set; }

        public IEnumerable<IdentifierResource> ManufacturerSkus { get; set; }
        public IEnumerable<IdentifierResource> VendorSkus { get; set; }
        public IEnumerable<IdentifierResource> UpcCodes { get; set; } 

        public RegionRefResource Region { get; set; }
        public EntityRefResource Entity { get; set; }

        public bool IsLinkedToCuratedProduct { get; set; }
        public bool IsSaleable { get; set; }
        public int Version { get; set; }
    }
}
