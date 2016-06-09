using System;
using System.Collections.Generic;
using System.Net;
using MagentoSync;
using MagentoSync.Controllers.Magento;
using MagentoSync.Models.Magento.Products;
using MagentoSync.Utilities;
using RestSharp;

namespace Tests.Utilities
{
	internal class TestHelper
	{
		//To troubleshoot issues with a specific Magento product, replace this value with a SKU from your Magento system
		private const string MagentoProductSku = "Configurable Product";

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
			if (response.StatusCode != HttpStatusCode.OK)
			{
				throw new Exception(string.Format("Unexpected HTTP status code. Expected {0}, returned {1}", HttpStatusCode.OK, response.StatusCode));
			}
		}

		private static ProductResource _testProduct;
		public static ProductResource TestProduct
		{
			get
			{
				return _testProduct ?? new ProductController(App.GetMagentoAuthToken()).GetProductBySku(MagentoProductSku);
			}
			set { _testProduct = value; }
		}

		public static ProductResource MockTestProduct
		{
			get
			{
				return new ProductResource()
				{
					id = 2049,
					sku = "Configurable Product",
					name = "Configurable Product",
					attribute_set_id = 4,
					price = new decimal(4.51),
					status = 1,
					visibility = 4,
					type_id = "simple",
					created_at = DateTime.Now,
					updated_at = DateTime.Now,
					product_links = new List<ProductLinkResource>(),
					options = new List<OptionResource>(),
					media_gallery_entries = new List<MediaGalleryEntryResource>()
						{
							new MediaGalleryEntryResource()
							{
								id = 3431,
								media_type = "image",
								label = "",
								position = 5,
								types = new List<string>()
								{
									"image",
									"small_image",
									"thumbnail",
									"swatch_image"
								},
								file = "/b/r/brand_new.jpg"
							}
						},
					tier_prices = new List<TierPriceResource>(),
					custom_attributes = new List<CustomAttributeRefResource>()
						{
							new CustomAttributeRefResource()
							{
								attribute_code = "manufacturer",
								value = "213"
							},
							new CustomAttributeRefResource()
							{
								attribute_code = ConfigReader.MappingCode,
								value = "M2039"
							},
							new CustomAttributeRefResource()
							{
								attribute_code = ConfigReader.MagentoImageCode,
								value = "/b/r/brand_new.jpg"
							},
							new CustomAttributeRefResource()
							{
								attribute_code = ConfigReader.MagentoColorCode,
								value = "49"
							}
						}
				};
			}
		}
	}
}
