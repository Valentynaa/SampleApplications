using System;

namespace MagentoSync.Models.EndlessAisle.Catalog
{
    public class CatalogItemResource
    {
        public string RmsId { get; set; }
        public string Slug { get; set; }
        public Guid CatalogItemId { get; set; }
        public bool IsArchived { get; set; }
    }
}