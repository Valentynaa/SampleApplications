using MagentoSync.Models.EndlessAisle.Catalog;

namespace MagentoSync.Controllers.EndlessAisle.Interfaces
{
	public interface ICatalogsController : IController
	{
		CatalogItemResource GetCatalogItem(string catalogItemId);
		string DeleteCatalogItem(string catalogItemId);
		CatalogItemResource CreateCatalogItem(CatalogItemResource catalogItem);
	}
}