using System;
using System.Collections.Generic;

// ReSharper disable InconsistentNaming

namespace MagentoSync.Models.Magento.Products
{
    [Serializable]
    public class MediaGalleryEntryResource
    {
        public int id { get; set; }
        public string media_type { get; set; }
        public string label { get; set; }
        public int position { get; set; }
        public bool disabled { get; set; }
        public List<string> types { get; set; }
        public string file { get; set; }
    }
}