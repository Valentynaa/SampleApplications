using MagentoSync.Models.EndlessAisle.Catalog;
using MagentoSync.Models.EndlessAisle.ProductLibrary;
using MagentoSync.Models.Magento.Products;
using MagentoSync.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MagentoSync.Controllers.EndlessAisle.Interfaces;
using MagentoSync.Controllers.Magento.Interfaces;

namespace MagentoSync.Mappers
{
	public class ProductMapper : BaseMapper
	{
		private readonly ICatalogsController _eaCatalogController;
		private readonly IProductLibraryController _eaProductController;
		private readonly IProductController _magentoProductController;

		public ProductMapper(ICatalogsController catalogsController, IProductLibraryController produtLibraryController, IProductController productController)
		{
			_eaCatalogController = catalogsController;
			_eaProductController = produtLibraryController;
			_magentoProductController = productController;
		}

		/// <summary>
		/// This function will return a list of Magento Products updated after a specific date
		/// This could be used to sync products between Magento and EA
		/// </summary>
		/// <param name="updatedAfter">Date to compare with product updated dates, use format YYYY-MM-DD HH:MM:SS (other formats may work, see Magento docs)</param>
		/// <returns>Magento products</returns>
		public IEnumerable<ProductResource> GetMagentoProductsUpdatedAfter(DateTime updatedAfter)
		{
			if(updatedAfter > DateTime.UtcNow)
				throw new ArgumentOutOfRangeException(nameof(updatedAfter));

			var dateString = updatedAfter.ToString(ConfigReader.MagentoSearchDateString, CultureInfo.InvariantCulture);

			var updatedProducts = _magentoProductController.SearchForProducts(ConfigReader.MagentoUpdatedAtProperty,
				dateString, ConfigReader.MagentoGreaterThanCondition);

			//Magento API doesn't seem to filter by time updated perfectly. Some items will be returned 
			//even if they were updated before the time specified so we manually filter the collection
			return updatedProducts.items.Where(x => x.updated_at > updatedAfter);
		}

        /**
		  * This function creates a master product
		  * 
		  * @param  productMapping          Mapping value, can be null
		  * @param  eaCategoryId            Identifier for a category in EA
		  * @param  eaClassificationTreeId  Identifier for a classification tree in ea
		  * @param  eaManufacturerId        Identifier for a manufacturer in EA
		  * @param  fields                  Fields representing changes to product details
		  * @param  assets                  Assets for product
		  *
		  * @return int                     Identifier of a product document in EA
		  */
        public int UpsertMasterProduct(string productMapping, int eaCategoryId, int eaClassificationTreeId, int? eaManufacturerId, List<FieldResource> fields, List<AssetResource> assets)
		{
			var productDocumentId = -1;

			if (productMapping != null)
			{
			    var slug = _eaCatalogController.GetCatalogItem(productMapping).Slug;

                productDocumentId = _eaProductController.UpdateMasterProduct(GetProductDocumentIdFromSlug(slug), 
					new RevisionEditResource
					{
						FieldValues = fields,
						Assets = assets,
						IdentifierGroups = null,
						ColorDefinitionId = null
					});
			}
			else
			{
				var requestBody = new ProductDocumentResource
				{
					Classification = new ProductClassificationRefResource
					{
						Id = eaCategoryId,
						TreeId = eaClassificationTreeId
					},
					OwnerEntityId = ConfigReader.EaCompanyId,
					RootRevision = new RootRevisionResource
					{
						FieldValues = fields,
						Assets = assets
					}
				};

			    if (eaManufacturerId != null)
			    {
			        requestBody.Manufacturer = new EntityRefResource { Id = (int) eaManufacturerId };
			    }

				productDocumentId = _eaProductController.CreateMasterProduct(requestBody).Id;
			}

			return productDocumentId;
		}

		/**
		  * This function takes a product resource and either creates a variation or updates one, if there is a mapping (UPSERT)
		  * 
		  * @param   magentoProduct      Magento product to be updated
		  * @param   productDocumentId   Identifier of parent
		  * @param   colorDefinitionId   Identifier for a color definition, a color for this product
		  * @param   fields              Fields representing changes to product details
		  * @param   assets              Assets for product
		  *
		  * @return  int                 Identifier of a variation in EA
		  */
		public int UpsertVariation(ProductResource magentoProduct, int productDocumentId, string colorDefinitionId,
			List<FieldResource> fields, List<AssetResource> assets)
		{
			//Update the variation instead of creating it, if applicable
			var variationId = -1;

			if (ProductHasMapping(magentoProduct))
			{
				//Get identifier of variation we are updating 
				var catalogItemId =
					GetAttributeByCode(magentoProduct.custom_attributes, ConfigReader.MappingCode).ToString();

			    var slug = _eaCatalogController.GetCatalogItem(catalogItemId).Slug;

				variationId = GetVariationIdFromSlug(slug);

				Guid? colorDefGuid;

				if (colorDefinitionId != null)
				{
					colorDefGuid = null;
				}
				else
				{
					colorDefGuid = new Guid(colorDefinitionId);
				}

				var variationUpdate = new RevisionEditResource
				{
					FieldValues = fields,
					Assets = assets,
					IdentifierGroups = null,
					ColorDefinitionId = colorDefGuid
				};

				variationId = _eaProductController.UpdateVariation(productDocumentId, variationId, variationUpdate);
			}
			else
			{
				var variation = new VariationResource()
				{
					FieldValues = fields,
					Assets = assets,
					ColorDefinitionId = new Guid(colorDefinitionId)
				};

				variationId = _eaProductController.CreateVariation(productDocumentId, variation).VariationId;
			}

			return variationId;
		}

		/**
		 * This function adds a product to your Catalog in EA given a slug
		 * 
		 * @param   slug            Slug of a Product in EA
		 *
		 * @return  string          Identifier of a created CatalogItem
		 */
		public string AddProductToEndlessAisle(string slug)
		{
       		var catalogItem = new CatalogItemResource
			{
				Slug = slug
			};

			return _eaCatalogController.CreateCatalogItem(catalogItem).CatalogItemId.ToString();
		}    

		/**
		 * Gets children of a configurable magento product
		 * 
		 * @param   magentoProduct          Magento product to be updated
		 *
		 * @return  List<ProductResource>   Child products
		 */
		public List<ProductResource> GetConfigurableProductChildren(ProductResource magentoProduct)
		{
			return _magentoProductController.GetConfigurableProductChildren(magentoProduct.sku);
		}

		/**
		 * Gets a magento product by sku
		 * 
		 * @param   sku                 Identifier for a product in magento
		 *
		 * @return  ProductResource     Magento product
		 */
		public ProductResource GetProductBySku(string sku)
		{
			return _magentoProductController.GetProductBySku(sku);
		}

		/**
		 * Calculates the SLUG for a product
		 * 
		 * @param   productDocumentId   Identifier for a Product Document in EA
		 * @param   variationId         Identifier for a Variation in EA
		 *
		 * @return  string              Slug value
		 */
		public string CalculateSlug(int productDocumentId, int? variationId)
		{
			var slug = string.Format("M{0}", productDocumentId);

			if (variationId != null)
				slug += string.Format("-V{0}", variationId);

			return slug;
		}

		/**
		 * Returns a Variation Id given a slug
		 * 
		 * @param   slug    Identifier for a product in EA, mapping value between EA and Magento
		 *
		 * @return  int     Identifier for a Variation in EA
		 */
		private static int GetVariationIdFromSlug(string slug)
		{
			if (slug.IndexOf("-", StringComparison.Ordinal) == -1)
				throw new Exception(string.Format("Slug {0} is not associated with a variation!", slug));

			return int.Parse(slug.Substring(slug.IndexOf("-", StringComparison.Ordinal) + 2));
		}

		/// <summary>
		/// Finds the quantity for a magento product based on the SKU provided. 
		/// If the product is not set  as "in stock" it will give a quantity of 0. 
		/// In the case that the quantity is returned as null,  then 1 is returned.
		/// </summary>
		/// <param name="sku">SKU to get inventory for</param>
		/// <returns>Quantity of product</returns>
		public int GetQuantityBySku(string sku)
		{
			var inventoryItem = _magentoProductController.GetInventoryBySku(sku);

			if (!inventoryItem.is_in_stock)
				return 0;

			return inventoryItem.qty == null ? 1 : (int) inventoryItem.qty;
		}
	}
}
