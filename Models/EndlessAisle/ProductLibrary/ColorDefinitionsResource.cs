using System;
using System.Collections.Generic;

namespace MagentoConnect.Models.EndlessAisle.ProductLibrary
{
    [Serializable]
    public class ColorDefinitionsResource
    {
        public IEnumerable<ColorDefinitionResource> ColorDefinitions { get; set; }
    }
}