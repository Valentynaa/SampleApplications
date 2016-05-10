using System;
using System.Collections.Generic;

namespace MagentoConnect.Models.EndlessAisle.ProductLibrary
{
    [Serializable]
    public class ColorDefinitionResource
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IList<int> ColorTagIds { get; set; }
        public IList<ColorTagResource> ColorTags { get; set; }
        public SwatchResource Swatch { get; set; }
    }
}