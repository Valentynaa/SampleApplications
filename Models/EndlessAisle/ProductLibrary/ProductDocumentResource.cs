using System;
using System.Collections.Generic;

namespace MagentoConnect.Models.EndlessAisle.ProductLibrary
{
    [Serializable]
    public class ProductDocumentResource
    {
        public int Id { get; set; }

        public RootRevisionResource RootRevision { get; set; }
        public ProductClassificationRefResource Classification { get; set; }
        public EntityRefResource Manufacturer { get; set; }
        
        public IList<RevisionGroupResource> RevisionGroups { get; set; }
        public EntityRefResource Owner { get; set; }

        public int OwnerEntityId { get; set; }

        public IList<ColorDefinitionResource> ColorDefinitions { get; set; }

        public DateTime CreatedUtc { get; set; }
        public DateTime? LastModifiedUtc { get; set; }
        public int Version { get; set; }
    }
}

