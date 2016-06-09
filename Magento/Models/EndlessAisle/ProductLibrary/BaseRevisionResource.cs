using System;
using System.Collections.Generic;

namespace MagentoSync.Models.EndlessAisle.ProductLibrary
{
    [Serializable]
    public class BaseRevisionResource
    {
        public BaseRevisionResource()
        {
            FieldValues = new List<FieldResource>();
            IdentifierGroups = new List<ProductIdentifierGroupResource>();
            Assets = new List<AssetResource>();
        }

        public IList<FieldResource> FieldValues { get; set; }
        public IList<ProductIdentifierGroupResource> IdentifierGroups { get; set; }
        public IList<AssetResource> Assets { get; set; }

        public Guid? ColorDefinitionId { get; set; }
    }
}