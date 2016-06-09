using System;

// ReSharper disable InconsistentNaming

namespace MagentoSync.Models.Magento.Category
{
    [Serializable]
    public class CategoryResource
    {
        public int id { get; set; }
        public int parent_id { get; set; }
        public string name { get; set; }
        public int position { get; set; }
        public int level { get; set; }
        public string children { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string path { get; set; }
        public bool include_in_menu { get; set; }
        
        /* Unused values due to complexity */
        public object available_sort_by = null;
        public object custom_attributes = null;
    }
}
