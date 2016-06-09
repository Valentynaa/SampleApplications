using System;
using System.Collections.Generic;

namespace MagentoSync.Models.EndlessAisle.ProductLibrary
{
    [Serializable]
    public class RevisionEditResource
    {
        public RevisionEditResource()
        {
            FieldValues = new List<FieldResource>();
            IdentifierGroups = new List<ProductIdentifierGroupResource>();
            Assets = new List<AssetResource>();
        }

        public List<FieldResource> FieldValues { get; set; }
        public List<ProductIdentifierGroupResource> IdentifierGroups { get; set; }
        public List<AssetResource> Assets { get; set; }
        public Guid? ColorDefinitionId { get; set; }
    }
}
