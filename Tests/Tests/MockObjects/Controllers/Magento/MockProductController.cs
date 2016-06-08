using MagentoConnect.Controllers.Magento;
using MagentoConnect.Models.Magento.Inventory;
using MagentoConnect.Models.Magento.Products;
using MagentoConnect.Utilities;
using System;
using System.Collections.Generic;
using OptionResource = MagentoConnect.Models.Magento.Products.OptionResource;

namespace Tests.MockObjects.Controllers.Magento
{
	public class MockProductController : BaseMockController, IProductController
	{
		public ProductResource GetProductBySku(string productSku)
		{
			return new ProductResource()
			{
				id = 2049,
				sku = productSku,
				name = productSku,
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
					}
				}
			};
		}

		public List<ProductResource> GetConfigurableProductChildren(string productSku)
		{
			return new List<ProductResource>()
			{
				new ProductResource()
				{
					id = 2049,
					sku = string.Format("{0}-XS-Green", productSku),
					name = string.Format("{0}-XS-Green", productSku),
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
						}
					}
				}
			};
		}
		
		public ProductSearchResource SearchForProducts(string property, string value, string condition)
		{
			return new ProductSearchResource()
			{
				items = new List<ProductResource>()
				{
					new ProductResource()
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
							}
						}
					}
				},
				total_count = 1
			};
		}

		public void AddCustomAttributeToProduct(ProductResource magentoProduct, List<int> categoryIds, string attrCode, string attrValue)
		{
		}

		public CatalogInventoryItemResource GetInventoryBySku(string sku)
		{
			return new CatalogInventoryItemResource()
			{
				item_id = 2049,
				product_id = 2049,
				stock_id = 1,
				qty = 50,
				is_in_stock = true,
				use_config_min_qty = true,
				min_qty = 0,
				use_config_min_sale_qty = 1,
				min_sale_qty = 1M,
				max_sale_qty = 1000M,
				use_config_backorders = true,
				backorders = 0,
				notify_stock_qty = 1,
				use_config_notify_stock_qty = true,
				qty_increments = 0M,
				use_config_manage_stock = true,
				manage_stock = true,
				stock_status_changed_auto = 0
			};
		}
	}
}
