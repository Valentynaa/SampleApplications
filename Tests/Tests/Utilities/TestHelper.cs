using System.Collections.Generic;
using MagentoConnect;
using MagentoConnect.Controllers;
using MagentoConnect.Controllers.Magento;
using MagentoConnect.Models.Magento.Products;
using MagentoConnect.Utilities;
using RestSharp;

namespace Tests.Utilities
{
    internal class TestHelper
	{
		//To troubleshoot issues with a specific Magento product, replace this value with a SKU from your Magento system
		private const string MagentoProductSku = "Configurable Product";
        private static ProductResource _testProduct;

        /// <summary>
        /// Creates a product update to ensure tests run correctly.
        /// </summary>
        /// <param name="productId">Magento ID for test product</param>
        /// <param name="productSku">Magento SKU for test product</param>
        /// <param name="categoryIds"> Magento category IDs test product is in</param>
        /// <param name="magentoAuthToken"></param>
        public static void CreateTestUpdate(string magentoAuthToken, int productId, string productSku, List<int> categoryIds)
		{
			var urlFormatter = new UrlFormatter();
			var endpoint = urlFormatter.MagentoCreateProductUrl();

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.POST);

			request.AddHeader("Authorization", string.Format("Bearer {0}", magentoAuthToken));
			request.AddHeader("Content-Type", "application/json");

			//Format update request body
			var updateProduct = new UpdateProductResource
			{
				product = new UpdateProductBodyResource
				{
					id = productId,
					sku = productSku,
					custom_attributes = new List<CustomAttributeRefResource>
					{
						new CustomAttributeRefResource { attribute_code = ConfigReader.MagentoCategoryCode, value = categoryIds },
					}
				}
			};

			request.AddJsonBody(updateProduct);

			var response = client.Execute(request);

			//Ensure we get the right code
			new BaseController().CheckStatusCode(response.StatusCode);
		}
		public static ProductResource TestProduct
		{
			get
			{
				return _testProduct ?? new ProductController(App.GetMagentoAuthToken()).GetProductBySku(MagentoProductSku);
			}
			set { _testProduct = value; }
		}
	}
}
