using System;

namespace MagentoSync.Models.EndlessAisle.ProductLibrary
{
    [Serializable]
    public class FieldResource
    {
        public int FieldDefinitionId { get; set; }
        public string LanguageInvariantValue { get; set; }
    }
}