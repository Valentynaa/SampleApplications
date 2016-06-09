using System;
using System.Collections.Generic;

namespace MagentoSync.Models.EndlessAisle.FieldDefinitions
{
    [Serializable]
    public class FieldDefinitionResource
    {
        public int Id { get; set; }
        public string StringId { get; set; }
        public string InputType { get; set; }
        public bool IsRequired { get; set; }
        public string LanguageInvariantUnit { get; set; }
        public string DisplayName { get; set; }
        public string Unit { get; set; }

        public List<OptionValueResource> Options { get; set; }

        [Obsolete]
        public string LanguageInvariantName { get; set; }
    }
}