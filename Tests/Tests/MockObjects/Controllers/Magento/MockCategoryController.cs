using MagentoConnect.Controllers.Magento;
using MagentoConnect.Models.Magento.Category;
using System;

namespace Tests.MockObjects.Controllers.Magento
{
	public class MockCategoryController : BaseMockController, ICategoryController
	{
		public CategoryResource GetCategory(int categoryId)
		{
			return new CategoryResource()
			{
				id = categoryId,
				parent_id = 0,
				children = "2",
				created_at = DateTime.Now,
				updated_at = DateTime.Now,
				level = 0,
				name = "Root Catalog",
				include_in_menu = true,
				path = "1",
				position = 0
			};
		}
	}
}
