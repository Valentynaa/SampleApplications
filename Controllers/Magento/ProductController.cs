using System.Collections.Generic;
using MagentoConnect.Models.Magento.CustomAttributes;
using MagentoConnect.Models.Magento.Inventory;
using MagentoConnect.Models.Magento.Products;
using Newtonsoft.Json;
using RestSharp;
using MagentoConnect.Utilities;

namespace MagentoConnect.Controllers.Magento
{
	public class ProductController : BaseController
	{
		public static string MagentoAuthToken;

		public ProductController(string magentoAuthToken)
		{
			MagentoAuthToken = magentoAuthToken;
		}

		/**
		 * Returns a ProductResource representing a item in your catalog
		 *
		 * @param   productSku  SKU of a Product to fetch
		 * @return              ProductResource Resource requested
		 */
		public ProductResource GetProductBySku(string productSku)
		{
			var endpoint = UrlFormatter.MagentoGetProductBySkuUrl(productSku);

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.GET);

			request.AddHeader("Authorization", string.Format("Bearer {0}", MagentoAuthToken));
		  
			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode);

			return JsonConvert.DeserializeObject<ProductResource>(response.Content);
		}

		/**
		 * Returns a list of children for a configurabke product
		 *
		 * @param   productSku              SKU of a Product
		 * @return  List<ProductResource>   Array of Products representing children
		 */
		public List<ProductResource> GetConfigurableProductChildren(string productSku)
		{
			var endpoint = UrlFormatter.MagentoConfigurableProductUrl(productSku);

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.GET);

			request.AddHeader("Authorization", string.Format("Bearer {0}", MagentoAuthToken));

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode);

			return JsonConvert.DeserializeObject<List<ProductResource>>(response.Content);
		}

		/**
		 * Returns an Attribute resource
		 *
		 * @param   attributeId                 Identifier for an Attribute
		 * @return  CustomAttributeResource     Attribute details
		 */
		public CustomAttributeResource GetAttribute(string attributeId)
		{
			var endpoint = UrlFormatter.MagentoAttributeUrl(attributeId);

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.GET);

			request.AddHeader("Authorization", string.Format("Bearer {0}", MagentoAuthToken));

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode);

			return JsonConvert.DeserializeObject<CustomAttributeResource>(response.Content);

		}

		/**
		 * Returns an list of Products in Magento matching search criteria
		 *
		 * @param   property    Property to search by
		 * @param   value       Value to search for
		 * @param   condition   Condition. See http://devdocs.magento.com/guides/v2.0/get-started/usage.html for a list of acceptable values
		 *
		 * @return  List<ProductResource>   Magento
		 */
		public ProductSearchResource SearchForProducts(string property, string value, string condition)
		{
			var endpoint = UrlFormatter.MagentoSearchProductsUrl(property, value, condition);

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.GET);

			request.AddHeader("Authorization", string.Format("Bearer {0}", MagentoAuthToken));

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode);

			return JsonConvert.DeserializeObject<ProductSearchResource>(response.Content);

		}

		/**
		 * Creates a custom property on a Magento product
		 *
		 * @param       magentoProduct  Magento product to update 
		 * @param       categoryIds     Magento categories, required for updating
		 * @param       attrCode        Code of value to add    
		 * @param       attrValue       Value to add
		 */
		public void AddCustomAttributeToProduct(ProductResource magentoProduct, List<int> categoryIds, string attrCode, string attrValue)
		{
			var endpoint = UrlFormatter.MagentoCreateProductUrl();

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.POST);

			request.AddHeader("Authorization", string.Format("Bearer {0}", MagentoAuthToken));
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
		/// <returns>Gatalog inventory item based on the SKU provided</returns>
		public CatalogInventoryItemResource GetInventoryBySku(string sku)
		{
			var endpoint = UrlFormatter.MagentoInventoryBySkuUrl(sku);

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.GET);

			request.AddHeader("Authorization", string.Format("Bearer {0}", MagentoAuthToken));

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode);

			return JsonConvert.DeserializeObject<CatalogInventoryItemResource>(response.Content);
		}
	}
}
