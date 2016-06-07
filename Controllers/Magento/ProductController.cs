using System.Collections.Generic;
using MagentoConnect.Models.Magento.CustomAttributes;
using MagentoConnect.Models.Magento.Inventory;
using MagentoConnect.Models.Magento.Products;
using Newtonsoft.Json;
using RestSharp;
using MagentoConnect.Utilities;

namespace MagentoConnect.Controllers.Magento
{
	public class ProductController : BaseController, IProductController
	{
		public string AuthToken { get; }

		public ProductController(string magentoAuthToken)
		{
			AuthToken = magentoAuthToken;
		}
		
		/// <summary>
		/// Returns a ProductResource representing a item in your catalog
		/// </summary>
		/// <param name="productSku">SKU of a Product to fetch</param>
		/// <returns>ProductResource Resource requested</returns>
		public ProductResource GetProductBySku(string productSku)
		{
			var endpoint = UrlFormatter.MagentoGetProductBySkuUrl(productSku);

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.GET);

			request.AddHeader("Authorization", string.Format("Bearer {0}", AuthToken));
		  
			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode);

			return JsonConvert.DeserializeObject<ProductResource>(response.Content);
		}

		/// <summary>
		/// Returns a list of children for a configurabke product
		/// </summary>
		/// <param name="productSku">SKU of a Product</param>
		/// <returns>List of Products representing children</returns>
		public List<ProductResource> GetConfigurableProductChildren(string productSku)
		{
			var endpoint = UrlFormatter.MagentoConfigurableProductUrl(productSku);

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.GET);

			request.AddHeader("Authorization", string.Format("Bearer {0}", AuthToken));

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode);

			return JsonConvert.DeserializeObject<List<ProductResource>>(response.Content);
		}

		/// <summary>
		/// Returns an list of Products in Magento matching search criteria
		/// </summary>
		/// <param name="property">Property to search by</param>
		/// <param name="value">Value to search for</param>
		/// <param name="condition">Condition. See http://devdocs.magento.com/guides/v2.0/get-started/usage.html for a list of acceptable values</param>
		/// <returns>List of Magento Products that match the filter criteria</returns>
		public ProductSearchResource SearchForProducts(string property, string value, string condition)
		{
			var endpoint = UrlFormatter.MagentoSearchProductsUrl(property, value, condition);

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.GET);

			request.AddHeader("Authorization", string.Format("Bearer {0}", AuthToken));

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode);

			return JsonConvert.DeserializeObject<ProductSearchResource>(response.Content);

		}

		/// <summary>
		/// Creates a custom property on a Magento product
		/// </summary>
		/// <param name="magentoProduct">Magento product to update </param>
		/// <param name="categoryIds">Magento categories, required for updating</param>
		/// <param name="attrCode">Code of value to add</param>
		/// <param name="attrValue">Value to add</param>
		public void AddCustomAttributeToProduct(ProductResource magentoProduct, List<int> categoryIds, string attrCode, string attrValue)
		{
			var endpoint = UrlFormatter.MagentoCreateProductUrl();

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.POST);

			request.AddHeader("Authorization", string.Format("Bearer {0}", AuthToken));
			request.AddHeader("Content-Type", "application/json");

			var customAttributes = new List<CustomAttributeRefResource>
				{
					new CustomAttributeRefResource {attribute_code = ConfigReader.MagentoCategoryCode, value = categoryIds},
					new CustomAttributeRefResource {attribute_code = attrCode, value = attrValue}
				};

			//Format update request body
			var updateProduct = new UpdateProductResource
			{           
				product = new UpdateProductBodyResource
				{
					id = magentoProduct.id,
					sku = magentoProduct.sku,
					custom_attributes = customAttributes
				}
			};

			request.AddJsonBody(updateProduct);

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode);
		}

		/// <summary>
		/// Gets the catalog inventory item based on the SKU provided.
		/// </summary>
		/// <param name="sku">SKU associated with the catalog inventory item</param>
		/// <returns>Catalog inventory item based on the SKU provided</returns>
		public CatalogInventoryItemResource GetInventoryBySku(string sku)
		{
			var endpoint = UrlFormatter.MagentoInventoryBySkuUrl(sku);

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.GET);

			request.AddHeader("Authorization", string.Format("Bearer {0}", AuthToken));

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode);

			return JsonConvert.DeserializeObject<CatalogInventoryItemResource>(response.Content);
		}
	}
}
