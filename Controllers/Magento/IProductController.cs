using System.Collections.Generic;
using MagentoConnect.Models.Magento.CustomAttributes;
using MagentoConnect.Models.Magento.Inventory;
using MagentoConnect.Models.Magento.Products;

namespace MagentoConnect.Controllers.Magento
{
	public interface IProductController : IController
	{
		ProductResource GetProductBySku(string productSku);
		List<ProductResource> GetConfigurableProductChildren(string productSku);
		CustomAttributeResource GetAttribute(string attributeId);
		ProductSearchResource SearchForProducts(string property, string value, string condition);
		void AddCustomAttributeToProduct(ProductResource magentoProduct, List<int> categoryIds, string attrCode, string attrValue);

		/// <summary>
		/// Gets the catalog inventory item based on the SKU provided.
		/// </summary>
		/// <param name="sku">SKU associated with the catalog inventory item</param>
		/// <returns>Catalog inventory item based on the SKU provided</returns>
		CatalogInventoryItemResource GetInventoryBySku(string sku);
	}
}