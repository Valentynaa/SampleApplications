using MagentoSync.Controllers.EndlessAisle;
using MagentoSync.Controllers.Magento;
using MagentoSync.Models.EndlessAisle.Catalog;
using MagentoSync.Models.EndlessAisle.ProductLibrary;
using MagentoSync.Models.Magento.Products;
using MagentoSync.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

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
		  * @param  eaManufacturerId        Identifier for a manufactrer in EA
		  * @param  fields                  Fields representing changes to product details
		  * @param  assets                  Assets for product
		  *
		  * @return int                     Identifier of a product document in EA
		  */
		public int UpsertMasterProduct(string productMapping, int eaCategoryId, int eaClassificationTreeId, int eaManufacturerId, List<FieldResource> fields, List<AssetResource> assets)
		{
			var productDocumentId = -1;

			if (productMapping != null)
			{
				productDocumentId = _eaProductController.UpdateMasterProduct(GetProductDocumentIdFromSlug(productMapping), 
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
					Manufacturer = new EntityRefResource
					{
						Id = eaManufacturerId
					},
					OwnerEntityId = ConfigReader.EaCompanyId,
					RootRevision = new RootRevisionResource
					{
						FieldValues = fields,
						Assets = assets
					}
				};

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
				var mappingSlug =
					GetAttributeByCode(magentoProduct.custom_attributes, ConfigReader.MappingCode).ToString();
				variationId = GetVariationIdFromSlug(mappingSlug);
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
			if (!Regex.IsMatch(slug, RegexPatterns.SlugPattern))
				throw new Exception(string.Format("\"{0}\" is in an invalid slug format.", slug));

			var catalogItem = new CatalogItemResource
			{
				Slug = slug
			};

			return _eaCatalogController.CreateCatalogItem(catalogItem).CatalogItemId.ToString();
		}    

		/**
		 * This function will parse a product document and add it to your catalog in EA
		 * 
		 * @param   productDocumentId   Identifier of a ProductDocument
		 *
		 * @return  List<string>        Identifiers of created CatalogItems
		 */
		public List<string> AddProductHierarchyToEndlessAisle(int productDocumentId)
		{
			if (productDocumentId < 1)
				throw new ArgumentOutOfRangeException(nameof(productDocumentId));

			var createdCatalogItems = new List<string>();

			var variationIds = GetVariationIdsForMasterProduct(productDocumentId);

			if (!variationIds.Any() && variationIds.Count == 0)
			{
				//No variations? Add it to EA as a simple product
				createdCatalogItems.Add(
					AddProductToEndlessAisle(CalculateSlug(productDocumentId, null)));
			}
			else
			{
				//Variations? Add them to EA
				createdCatalogItems.AddRange(
					variationIds.Select(
						variationId =>
							AddProductToEndlessAisle(CalculateSlug(productDocumentId, variationId))));
			}

			return createdCatalogItems;
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

		/**
		 * Gets Variations from a product structure
		 * 
		 * @param   productDocumentId               Identifier for a Product Document in EA
		 *
		 * @return  IList<RevisionGroupResource>    Revisions on the Product Document
		 */
		public List<int> GetVariationIdsForMasterProduct(int productDocumentId)
		{
			var revisionGroups = _eaProductController.GetProductHierarchy(productDocumentId).RevisionGroups;
			var variationIds = new List<int>();

			foreach (var group in revisionGroups)
			{
				if (@group.VariationId == null) continue;

				if (!variationIds.Contains(@group.VariationId.Value))
					variationIds.Add(@group.VariationId.Value);
			}

			return variationIds;
		}

		/// <summary>
		/// Finds the catalog items that match the provided slug and returns the first item's catalogItemId.
		/// 
		/// NOTE: 
		///		Multiple catalog items could correspond to a single slug, but for now only the first item
		///		will have the pricing updated in EA.
		/// </summary>
		/// <param name="slug">Slug to find catalog items for.</param>
		/// <returns>CatalogItemId of first item or null if no items are found.</returns>
		public string GetCatalogItemIdBySlug(string slug)
		{
			var item = _eaCatalogController.GetCatalogItemsBySlug(slug).FirstOrDefault();
			
			return item?.CatalogItemId.ToString();
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
