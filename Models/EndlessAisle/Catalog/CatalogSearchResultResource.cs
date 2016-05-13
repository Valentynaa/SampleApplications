using System;
using System.Collections.Generic;

namespace MagentoConnect.Models.EndlessAisle.Catalog
{
	public class CatalogSearchResultResource
	{
		public IEnumerable<CatalogItemResource> Items { get; set; }
	}
}
