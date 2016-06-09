using MagentoSync.Controllers.EndlessAisle;
using MagentoSync.Models.EndlessAisle.Catalog;
using System;
using System.Collections.Generic;

namespace Tests.MockObjects.Controllers.EndlessAisle
{
	public class MockCatalogsController : BaseMockController, ICatalogsController
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
	}
}
