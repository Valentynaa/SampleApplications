﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagentoConnect.Controllers.Magento;
using MagentoConnect.Models.Magento.Category;

namespace Tests.MockObjects.Controllers.Magento
{
	public class MockCategoryController : ICategoryController
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

		public string AuthToken
		{
			get
			{
				return "WHdvc0h9b3NYQW9zWHZvc1h2JSYBAyc_PhRWBSAYLUFsIBkSMEA9MRYVWAcsPCQQaD8NJiBPLSUeFVgXGjEBJA4bX0EaBBtAOR8I";
			}
		}
	}
}
