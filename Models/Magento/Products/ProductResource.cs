using System;
using System.Collections.Generic;

// ReSharper disable InconsistentNaming

namespace MagentoConnect.Models.Magento.Products
{
    [Serializable]
    public class ProductResource
    {
        public int id { get; set; }

        public string sku { get; set; }
        public string name { get; set; }
        public int attribute_set_id { get; set; }
        public decimal price { get; set; }
        public int status { get; set; }
        public int visibility { get; set; }
        public string type_id { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public ExtensionAttributes extension_attributes { get; set; } //This property is not covered
        public List<ProductLinkResource> product_links { get; set; }
        public List<OptionResource> options { get; set; } 
        public List<MediaGalleryEntryResource> media_gallery_entries { get; set; }
        public List<TierPriceResource> tier_prices { get; set; } 
        public List<CustomAttributeRefResource> custom_attributes { get; set; }

        public int? weight { get; set; }
    }
}