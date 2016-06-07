using System;
using System.Collections.Generic;
using MagentoConnect.Controllers.EndlessAisle;
using MagentoConnect.Models.EndlessAisle.Catalog;

namespace Tests.MockObjects.Controllers.EndlessAisle
{
	public class MockCatalogsController : ICatalogsController
	{
		public CatalogItemResource GetCatalogItem(string catalogItemId)
		{
			return new CatalogItemResource()
			{
				Slug = "M2039",
				CatalogItemId = catalogItemId == null? new Guid() : new Guid(catalogItemId),
				IsArchived = false,
				RmsId = null
			};
		}

		public IEnumerable<CatalogItemResource> GetCatalogItemsBySlug(string slug)
		{
			return new List<CatalogItemResource>()
			{
				new CatalogItemResource()
				{
					CatalogItemId = new Guid(),
					IsArchived = false,
					RmsId = null,
					Slug = slug
				}
			};
		}

		public string DeleteCatalogItem(string catalogItemId)
		{
			return null;
		}

		public CatalogItemResource CreateCatalogItem(CatalogItemResource catalogItem)
		{
			return new CatalogItemResource()
			{
				CatalogItemId = new Guid(),
				IsArchived = catalogItem.IsArchived,
				RmsId = catalogItem.RmsId,
				Slug = catalogItem.Slug
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
