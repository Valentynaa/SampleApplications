using System;

namespace MagentoSync.Models.EndlessAisle.ProductLibrary
{
    [Serializable]
    public class RevisionContext
    {
        public int? EntityId { get; set; }
        public int? ParentEntityId { get; set; }
        public string CountryCode { get; set; }
        public string StateCode { get; set; }
    }
}