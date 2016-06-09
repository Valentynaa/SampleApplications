using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;

// ReSharper disable InconsistentNaming

namespace MagentoSync.Models.Magento.CustomAttributes
{
    [Serializable]
    public class CustomAttributeResource
    {
        public IEnumerable<OptionResource> options { get; set; }

        public bool is_wysiwyg_enabled { get; set; }
        public bool is_html_allowed_on_front { get; set; }
        public bool used_for_sort_by { get; set; }
        public bool is_filterable { get; set; }
        public bool is_filterable_in_search { get; set; }
        public bool is_used_in_grid { get; set; }
        public bool is_visible_in_grid { get; set; }
        public bool is_filterable_in_grid { get; set; }
        public int position { get; set; }
        public string is_searchable { get; set; }
        public string is_visible_in_advanced_search { get; set; }
        public string is_comparable { get; set; }
        public string is_used_for_promo_rules { get; set; }
        public string is_visible_on_front { get; set; }
        public string used_in_product_listing { get; set; }
        public bool is_visible { get; set; }
        public string scope { get; set; }
        public int attribute_id { get; set; }
        public string attribute_code { get; set; }
        public string frontend_input { get; set; }
        public string entity_type_id { get; set; }
        public bool is_required { get; set; }
        public bool is_user_defined { get; set; }
        public string default_frontend_label { get; set; }
        public string backend_type { get; set; }
        public string source_model { get; set; }
        public string default_value { get; set; }
        public string is_unique { get; set; }

        /* Unused values due to complexity */
        public object validation_rules = null;
        public object frontend_labels = null;
        public IEnumerable<object> apply_to = null;
    }
}
