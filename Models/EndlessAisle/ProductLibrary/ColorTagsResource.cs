using System;
using System.Collections.Generic;

namespace MagentoConnect.Models.EndlessAisle.ProductLibrary
{
    [Serializable]
    public class ColorTagsResource
    {
        public List<ColorTagResource> ColorTags { get; set; }
    }
}