using System;
using System.Collections.Generic;

namespace MagentoSync.Models.EndlessAisle.Catalog
{
	public class CatalogSearchResultResource
	{
		public IEnumerable<CatalogItemResource> Items { get; set; }
	}
}
