using System;
using System.Collections.Generic;
using System.Linq;
using MagentoConnect.Models.Magento.Products;
using MagentoConnect.Utilities;
using MagentoConnect.Models.EndlessAisle.ProductLibrary;
using MagentoConnect.Mappers;
using MagentoConnect.Models.EndlessAisle.Orders;

namespace MagentoConnect
{
	public class App
	{
		private const string UserAffirmativeString = "y";

		//Caching to make things a bit faster
		private static string _cachedEaAuthToken;
		private static string _cachedMagentoAuthToken;

		private static ProductMapper _productMapper;
		private static AssetMapper _assetMapper;
		private static AvailabilityMapper _availabilityMapper;
		private static ColorMapper _colorMapper;
		private static FieldMapper _fieldMapper;
		private static PricingMapper _pricingMapper;
		private static OrderMapper _orderMapper;
		private static EntityMapper _entityMapper;
		private static CustomerMapper _customerMapper;

		/**
		 * This console app will sync products created in the last X time
		 */
		public static void Main()
		{           
			_cachedEaAuthToken = GetEaAuthToken();
			_cachedMagentoAuthToken = GetMagentoAuthToken();

			_productMapper = new ProductMapper(_cachedMagentoAuthToken, _cachedEaAuthToken);
			_assetMapper = new AssetMapper(_cachedMagentoAuthToken, _cachedEaAuthToken);
			_availabilityMapper = new AvailabilityMapper(_cachedMagentoAuthToken, _cachedEaAuthToken);
			_colorMapper = new ColorMapper(_cachedMagentoAuthToken, _cachedEaAuthToken);
			_fieldMapper = new FieldMapper(_cachedMagentoAuthToken, _cachedEaAuthToken);
			_pricingMapper = new PricingMapper(_cachedMagentoAuthToken, _cachedEaAuthToken);
			_orderMapper = new OrderMapper(_cachedMagentoAuthToken, _cachedEaAuthToken);
			_entityMapper = new EntityMapper(_cachedMagentoAuthToken, _cachedEaAuthToken);
			_customerMapper = new CustomerMapper(_cachedMagentoAuthToken, _cachedEaAuthToken);

			bool doOrderSync;
			bool productsSynced = ProductSync();
			if (productsSynced)
			{
				Console.WriteLine("Products successfully synced");
				doOrderSync = true;
			}
			else
			{
				Console.WriteLine("An error occurred while syncing products to Endless Aisle. Check errorLog.txt for more details.");
				Console.WriteLine("Continue on to synchronizing Orders to Magento?");
				doOrderSync = Console.ReadKey().ToString().Equals(UserAffirmativeString, StringComparison.OrdinalIgnoreCase);
			}

			//Order syncing
			if (doOrderSync)
			{
				bool ordersSynced = OrderSync();
				if (ordersSynced)
				{
					Console.WriteLine("Orders successfully synced");
				}
				else
				{
					Console.WriteLine("An error occurred while syncing orders to Magento. Check errorLog.txt for more details.");
				}
			}
			Console.WriteLine("Press enter to exit...");
			Console.ReadLine();
		}

		/// <summary>
		/// Performs the sync for Endless Aisle orders to Magento.
		/// 
		/// For each of the EA orders since the last sync, a cart is made in Magento
		/// that has all of the order products added to it. The cart then has its 
		/// shipping information set based on the location data in EA and an order is
		/// created from the cart.
		/// 
		/// NOTE:
		///		If the location data required cannot be found for the shipping and 
		///		billing information, the customer data will be used instead. If the
		///		customer does not have the required fields set in Magento an error 
		///		will occur.
		/// </summary>
		/// <returns>If the sync was susscessful</returns>
		private static bool OrderSync()
		{
			try
			{
				DateTime lastSync = GetTimeForSync(Log.OrderSync);
				List<OrderResource> ordersToCreate = _orderMapper.GetEaOrdersCreatedAfter(lastSync).ToList();

				if (!ordersToCreate.Any())
				{
					Console.WriteLine("No orders to update.");
				}
				else
				{
					foreach (var order in ordersToCreate)
					{
						lastSync = order.CreatedDateUtc > lastSync ? order.CreatedDateUtc : lastSync;
						int cartId = _orderMapper.CreateCustomerCart();
						_orderMapper.AddOrderItemsToCart(order.Id.ToString(), cartId);
						_orderMapper.SetShippingAndBillingInformationForCart(cartId, _entityMapper.MagentoRegion, _entityMapper.EaLocation, _customerMapper.MagentoCustomer);
						int orderCreatedId = _orderMapper.CreateOrderForCart(cartId);
						Console.WriteLine("Order with ID {0} in Magento has been created from order {1} in Endless Aisle.", orderCreatedId, order.Id);
					}
					LogUtility.Write(Log.OrderSync, string.Format("Orders successfully synced. Last order synced was created at {0}", lastSync));
				}
				
				return true;
			}
			catch (Exception ex)
			{
				LogException(ex);

				//Uncomment if you want exceptions thrown at runtime.
				//throw;
			}
			return false;
		}

		/// <summary>
		/// Performs the sync for Magento products orders to Endless Aisle.
		/// </summary>
		/// <returns>If the sync was susscessful</returns>
		private static bool ProductSync()
		{
			try
			{
				DateTime lastSync = GetTimeForSync(Log.ProductSync);
				List<ProductResource> productsToUpdate = _productMapper.GetMagentoProductsUpdatedAfter(lastSync).ToList();
				
				if (!productsToUpdate.Any())
				{
					Console.WriteLine("No products to update.");
				}

				foreach (var newProduct in productsToUpdate)
				{
					lastSync = newProduct.updated_at > lastSync ? newProduct.updated_at : lastSync;
					UpsertProduct(_productMapper.GetProductBySku(newProduct.sku));
					Console.WriteLine("Product with SKU {0} has been updated.", newProduct.sku);
				}
				
				LogUtility.Write(Log.ProductSync, string.Format("Products successfully synced. Last product synced was updated at {0}", lastSync));
				return true;
			}
			catch (Exception ex)
			{
				LogException(ex);

				//Uncomment if you want exceptions thrown at runtime.
				//throw;
			}
			return false;
		}

		/// <summary>
		/// Gets the timestamp in message of the last log of a specified log type
		/// </summary>
		/// <param name="logType">Log type to get time for</param>
		/// <returns>Time to sync from</returns>
		private static DateTime GetTimeForSync(Log logType)
		{
			DateTime lastSync;
			DateTime lastLog;
			if (LogUtility.TryGetLastLog(logType, out lastLog) && LogUtility.TryGetTimeInformationForLog(logType, lastLog, out lastSync))
				return lastSync;
			return DateTime.Now.AddHours(-1);
		}

		private static void LogException(Exception exception)
		{
			LogUtility.Write(Log.Error, exception.Message);
		}

		/**
		 * Main driver, updates a product
		 * 
		 * @param   sku     Magento product SKU
		 */
		private static void UpsertProduct(ProductResource magentoProduct)
		{
			//Skip children of configurable products - they will be updated with the parent
			if (magentoProduct.weight != null) return;

			switch (magentoProduct.type_id)
			{
				case "simple":
					UpsertSimpleMagentoProductToEa(magentoProduct);
					break;
				case "virtual":
					UpsertSimpleMagentoProductToEa(magentoProduct);
					break;
				case "configurable":
					UpsertConfigurableMagentoProductToEa(magentoProduct);
					break;
				default:
					Console.WriteLine(
							"Product {0} is of type {1}, which is not supported. Only Configrable and Simple products are supported.",
							magentoProduct.sku, magentoProduct.type_id);
					break;
			}
		}

		/// <summary>
		/// This function provides an example of how to create a product in EA given a Magento SKU
		/// This function specifically creates a Master Product from a *SIMPLE* Magento Product
		/// 
		/// NOTE: 
		///		This function requires you prepare the product by ensuring it has:
		///		- Assigned Manufacturer that matches one in EA
		///		- The first category must also be one defined in EA
		///		- If there are multiple Categories or Manufacturers, the first one will be used
		///		- When the product is sucessfully created, an attribute (MappingCode) will be added to each Magento product
		///		- If MULTIPLE images are provided, the BASE Magento image will be used as the hero shot
		/// </summary>
		/// <param name="magentoProduct">Magento product object</param>
		/// <returns>ProductDocumentId Identifier of created Product Document</returns>
		private static int UpsertSimpleMagentoProductToEa(ProductResource magentoProduct)
		{
			//Get fields and assets
			var fields = _fieldMapper.ParseFieldsFromProduct(magentoProduct);
			var assets = _assetMapper.ParseAssetsFromProduct(magentoProduct);

			var eaCategoryId = _fieldMapper.GetMatchingCategory(magentoProduct.custom_attributes);
			var eaManufacturerId = _fieldMapper.GetMatchingManufacturer(magentoProduct.custom_attributes);

			var productMapping = _fieldMapper.GetProductMapping(magentoProduct);

			//Upsert product
			var productDocumentId = _productMapper.UpsertMasterProduct(productMapping, eaCategoryId, ConfigReader.EaClassificationTreeId,
				eaManufacturerId, fields, assets);

			var slug = _productMapper.CalculateSlug(productDocumentId, null);

			string catalogItemId;
			if (productMapping == null)
			{
				//Add newly created product library product to your catalog
				catalogItemId = _productMapper.AddProductToEndlessAisle(slug);

				//Set the mapping SLUG on the product so we know it is mapped
				_fieldMapper.CreateMappingForProduct(magentoProduct, slug);
			}
			else
			{
				catalogItemId = _productMapper.GetCatalogItemIdBySlug(slug);
			}

			if (catalogItemId != null)
			{
				_pricingMapper.UpsertPricingForCatalogItem(catalogItemId, magentoProduct.price);

				//Create availability for product at company level
				_availabilityMapper.UpsertAvailabilityForCatalogItem(catalogItemId, _productMapper.GetQuantityBySku(magentoProduct.sku));
			}
			else
			{
				Console.WriteLine(
					"Product {0} unable to have its price or inventory updated since no catalog items could be found for mapping code {1}.",
					magentoProduct.sku, slug);
			}

			//If we have colors defined, add it as a color definition (if its not already added)
			var colorId = _colorMapper.GetMagentoColorAttribute(magentoProduct);

			if (colorId != null)
			{
				_colorMapper.UpsertColorDefinitions(productDocumentId, int.Parse(colorId));
			}

			SetHeroShot(magentoProduct, assets, slug);

			return productDocumentId;
		}

		/// <summary>
		/// This function provides an example of how to create a product structure in EA given a Magento SKU
		/// This function specifically creates a Master Product with Variations from a *CONFIGURABLE* Magento Product
		///
		/// NOTE: 
		///		This function requires you prepare the product by ensuring it has an
		///		assigned Manufacturer that matches one in EA
		///		The first category must also be one defined in EA
		///		If there are multiple Categories or Manufacturers, the first one will be used
		///		When the product is sucessfully created, an attribute (MappingCode) will be added to each Magento product
		/// 
		/// LIMITATION: 
		///		This app CANNOT remove child products from a variation if they are removed from a configurable product
		/// </summary>
		/// <param name="magentoProduct">Magento product object</param>
		private static void UpsertConfigurableMagentoProductToEa(ProductResource magentoProduct)
		{
			string colorDefinitionId = null;
			var productDocumentId = -1;

			//Get the child products
			var childProducts = _productMapper.GetConfigurableProductChildren(magentoProduct);

			//Update Master Product, if applicable
			productDocumentId = UpsertSimpleMagentoProductToEa(magentoProduct);

			foreach (var childProduct in childProducts)
			{
				var childProductObj = _productMapper.GetProductBySku(childProduct.sku);

				//Get fields + assets for child
				var fields = _fieldMapper.ParseFieldsFromProduct(childProduct);
				var assets = _assetMapper.ParseAssetsFromProduct(childProductObj);

				//If we have colors defined, add it as a color definition (if its not already added)
				var colorId = _colorMapper.GetMagentoColorAttribute(childProduct);

				if (colorId != null)
				{
					colorDefinitionId = _colorMapper.UpsertColorDefinitions(productDocumentId, int.Parse(colorId));
				}

				//Upsert variation
				var variationId = _productMapper.UpsertVariation(childProduct, productDocumentId, colorDefinitionId, fields,
					assets);

				var slug = _productMapper.CalculateSlug(productDocumentId, variationId);
				var productMapping = _fieldMapper.GetProductMapping(childProduct);

				SetHeroShot(childProductObj, assets, slug);

				string catalogItemId;
				if (productMapping == null)
				{
					//Add to Endless Aisle
					catalogItemId = _productMapper.AddProductToEndlessAisle(slug);

					//Set the mapping on the product to make updating it easier next time
					_fieldMapper.CreateMappingForProduct(childProduct, slug);
				}
				else
				{
					catalogItemId = _productMapper.GetCatalogItemIdBySlug(slug);
				}

				if (catalogItemId != null)
				{
					_pricingMapper.UpsertPricingForCatalogItem(catalogItemId, magentoProduct.price);

					//Create availability for product at company level
					_availabilityMapper.UpsertAvailabilityForCatalogItem(catalogItemId, _productMapper.GetQuantityBySku(magentoProduct.sku));
				}
				else
				{
					Console.WriteLine(
						"Product {0} unable to have its price or inventory updated since no catalog items could be found for mapping code {1}.",
						magentoProduct.sku, slug);
				}
			}
		}
		
		/**
		 * This function sets a hero shot
		 * 
		 * @param   magentoAssets   Magento assets
		 * @paran   eaAssets        EA product assets
		 * @param   slug            Identifier for a product in EA
		 */
		private static void SetHeroShot(ProductResource magentoProduct, IReadOnlyList<AssetResource> eaAssets, string slug)
		{
			var heroShot = _assetMapper.GetHeroShot(magentoProduct);

			if (magentoProduct.media_gallery_entries == null || eaAssets == null || heroShot == null) return;

			//Get the appropriate asset by matching, lists were created in sync so position of one is position of other
			//Not a fantastic way of doing this, I know
			for (var i = 0; i < magentoProduct.media_gallery_entries.Count; i++)
			{
				if (magentoProduct.media_gallery_entries[i] == heroShot)
				{
					_assetMapper.SetHeroShot(slug, eaAssets[i].Id);
				}
			}
		}

		/**
		 * Returns a string representing an authorization token for Magento
		 * This value is required when creating any Magento controllers
		 * If there is a previously cached Magento token, that will be returned instead
		 * 
		 * @return  _cachedMagentoAuthToken     A magento authorization token
		 */
		public static string GetMagentoAuthToken()
		{
			if (!string.IsNullOrEmpty(_cachedMagentoAuthToken))
			{
				return _cachedMagentoAuthToken;
			}

			var magentoAuthController = new Controllers.Magento.AuthController();

			//Read authentication information from config
			var magentoUsername = ConfigReader.MagentoUserName;
			var magentoPassword = ConfigReader.MagentoPassword;

			var magentoCredentials = new Models.Magento.Authentication.AuthenticationCredentialsResource
			{
				username = magentoUsername,
				password = magentoPassword
			};

			_cachedMagentoAuthToken = magentoAuthController.Authenticate(magentoCredentials);

			return _cachedMagentoAuthToken;
		}

		/**
		 * Returns a string representing an authorization token for Endless Aisle
		 * This value is required when creating any EA controllers
		 * If there is a previously cached EA token, that will be returned instead
		 * 
		 * @return  _cachedEaAuthToken  An Endless Aisle authorization token
		 */
		public static string GetEaAuthToken()
		{
			if (!string.IsNullOrEmpty(_cachedEaAuthToken))
			{
				return _cachedEaAuthToken;
			}

			var eaAuthController = new Controllers.EndlessAisle.AuthController();

			//Read authentication information from config
			var eaClientSecret = ConfigReader.EaClientSecret;
			var eaClientId = ConfigReader.EaClientId;
			var eaUsername = ConfigReader.EaUsername;
			var eaPassword = ConfigReader.EaPassword;
			var eaGrantType = ConfigReader.EaGrantType;

			var eaCredentials = new Models.EndlessAisle.Authentication.AuthenticationCredentialsResource()
			{
				client_id = eaClientId,
				client_secret = eaClientSecret,
				grant_type = eaGrantType,
				username = eaUsername,
				password = eaPassword
			};

			_cachedEaAuthToken = eaAuthController.Authenticate(eaCredentials);

			return _cachedEaAuthToken;
		}
	}
}
