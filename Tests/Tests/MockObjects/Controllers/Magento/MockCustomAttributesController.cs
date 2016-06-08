using MagentoConnect.Controllers.Magento;
using MagentoConnect.Models.Magento.CustomAttributes;
using System.Collections.Generic;

namespace Tests.MockObjects.Controllers.Magento
{
	public class MockCustomAttributesController : BaseMockController, ICustomAttributesController
	{
		/// <summary>
		/// ID for a mapped color in GetCustomAttributeIfExists
		/// </summary>
		public static int MappedColorId
		{
			get { return 49; }
		}

		public CustomAttributeResource GetCustomAttributeIfExists(string attributeCode)
		{
			return new CustomAttributeResource()
			{
				is_filterable = true,
				is_used_in_grid = true,
				is_filterable_in_grid = true,
				position = 0,
				is_searchable = "1",
				apply_to = new List<object>(),
				is_visible_in_advanced_search = "1",
				is_comparable = "1",
				is_visible = true,
				scope = "global",
				is_used_for_promo_rules = "0",
				is_visible_on_front = "0",
				used_in_product_listing = "0",
				attribute_id = 80,
				attribute_code = attributeCode,
				frontend_input = "select",
				entity_type_id = "4",
				is_required = true,
				options = new List<OptionResource>()
				{
					new OptionResource()
					{
						value = "",
						label = ""
					},
					new OptionResource()
					{
						label = "Sony",
						value = "213"
					},
					new OptionResource()	//For ColorMapper tests
					{
						label = "Black",
						value = "49"
					}
				},
				is_user_defined = true,
				default_frontend_label = "Manufacturer",
				backend_type = "int",
				source_model = "Magento\\Eav\\Model\\Entity\\Attribute\\Source\\Table",
				default_value = "213",
				is_unique = "0"
			};
		}

		public CustomAttributeResource CreateCustomAttribute(string attributeCode, string inputType, bool isRequired, string label)
		{
			return new CustomAttributeResource()
			{
				is_filterable = true,
				is_used_in_grid = true,
				is_filterable_in_grid = true,
				position = 0,
				is_searchable = "1",
				apply_to = new List<object>(),
				is_visible_in_advanced_search = "1",
				is_comparable = "1",
				is_visible = true,
				scope = "global",
				is_used_for_promo_rules = "0",
				is_visible_on_front = "0",
				used_in_product_listing = "0",
				attribute_id = 80,
				attribute_code = attributeCode,
				frontend_input = inputType,
				entity_type_id = "4",
				is_required = isRequired,
				options = new List<OptionResource>()
				{
					new OptionResource()
					{
						value = "",
						label = ""
					},
					new OptionResource()
					{
						label = "Sony",
						value = "213"
					}
				},
				is_user_defined = true,
				default_frontend_label = label,
				backend_type = "int",
				source_model = "Magento\\Eav\\Model\\Entity\\Attribute\\Source\\Table",
				default_value = "213",
				is_unique = "0"
			};
		}
	}
}
