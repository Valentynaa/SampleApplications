using System.Collections.Generic;
using MagentoSync.Models.EndlessAisle.Catalog;

namespace MagentoSync.Controllers.EndlessAisle
{
	public interface ICatalogsController : IController
	{
		CatalogItemResource GetCatalogItem(string catalogItemId);

		/// <summary>
		/// Gets the catalog items from the slug provided.
		/// </summary>
		/// <param name="slug">Slug to get items for.</param>
		/// <returns>Catalog items for the slug provided.</returns>
		IEnumerable<CatalogItemResource> GetCatalogItemsBySlug(string slug);

		string DeleteCatalogItem(string catalogItemId);
		CatalogItemResource CreateCatalogItem(CatalogItemResource catalogItem);
	}
}