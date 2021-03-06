﻿using MagentoSync.Controllers;
using MagentoSync.Controllers.EndlessAisle;
using MagentoSync.Controllers.Magento;
using MagentoSync.Mappers;
using MagentoSync.Models.Authentication;
using MagentoSync.Models.Magento.Products;
using MagentoSync.Utilities;
using System;
using System.Linq;

namespace MagentoSync
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

			var controllerFactory = new ControllerFactory(_cachedMagentoAuthToken, _cachedEaAuthToken);

			var assetsController = controllerFactory.CreateController(ControllerType.Assets) as AssetsController;
			var availabilityController = controllerFactory.CreateController(ControllerType.Availability) as AvailabilityController;
			var catalogsController = controllerFactory.CreateController(ControllerType.Catalogs) as CatalogsController;
			var entitiesController = controllerFactory.CreateController(ControllerType.Entities) as EntitiesController;
			var fieldController = controllerFactory.CreateController(ControllerType.FieldDefinition) as FieldDefinitionController;
			var ordersController = controllerFactory.CreateController(ControllerType.Orders) as OrdersController;
			var pricingController = controllerFactory.CreateController(ControllerType.Pricing) as PricingController;
			var productLibraryController = controllerFactory.CreateController(ControllerType.ProductLibrary) as ProductLibraryController;

			var cartController = controllerFactory.CreateController(ControllerType.Cart) as CartController;
			var attributesController = controllerFactory.CreateController(ControllerType.CustomAttributes) as CustomAttributesController;
			var customerController = controllerFactory.CreateController(ControllerType.Customer) as CustomerController;
			var productController = controllerFactory.CreateController(ControllerType.Product) as ProductController;
			var regionController = controllerFactory.CreateController(ControllerType.Region) as RegionController;

			_productMapper = new ProductMapper(catalogsController, productLibraryController, productController);
			_assetMapper = new AssetMapper(assetsController, productLibraryController, catalogsController);
			_availabilityMapper = new AvailabilityMapper(availabilityController);
			_colorMapper = new ColorMapper(attributesController, productLibraryController);
			_fieldMapper = new FieldMapper(productLibraryController, productController, fieldController, catalogsController, attributesController);
			_pricingMapper = new PricingMapper(pricingController);
			_orderMapper = new OrderMapper(ordersController, catalogsController, cartController, productController);
			_entityMapper = new EntityMapper(entitiesController, regionController);
			_customerMapper = new CustomerMapper(customerController);

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
			    Console.WriteLine(ordersSynced
			        ? "Orders successfully synced"
			        : "An error occurred while syncing orders to Magento. Check errorLog.txt for more details.");
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
				var lastSync = GetTimeForSync(Log.OrderSync);
				var ordersToCreate = _orderMapper.GetEaOrdersCreatedAfter(lastSync).ToList();

				if (!ordersToCreate.Any())
				{
					Console.WriteLine("No orders to update.");
				}
				else
				{
					foreach (var order in ordersToCreate)
					{
						lastSync = order.CreatedDateUtc > lastSync ? order.CreatedDateUtc : lastSync;
						var cartId = _orderMapper.CreateCustomerCart();
						_orderMapper.AddOrderItemsToCart(order.Id.ToString(), cartId);
						_orderMapper.SetShippingAndBillingInformationForCart(cartId, _entityMapper.MagentoRegion, _entityMapper.EaLocation, _customerMapper.MagentoCustomer);
						var orderCreatedId = _orderMapper.CreateOrderForCart(cartId);
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
				var lastSync = GetTimeForSync(Log.ProductSync);
				var productsToUpdate = _productMapper.GetMagentoProductsUpdatedAfter(lastSync).ToList();
				
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
		
		/// <summary>
		/// Main driver, updates a product
		/// </summary>
		/// <param name="magentoProduct">Magento product SKU</param>
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
		///		- The first category must also be one defined in EA
		///     - If manufacturer exists, it will be mapped to a manufacturer in EA
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
			var assets = _assetMapper.ParseAssetsFromProduct(magentoProduct, ConfigReader.MagentoStorageConfiguration);

			var eaCategoryId = _fieldMapper.GetMatchingCategory(magentoProduct.custom_attributes);
			var eaManufacturerId = _fieldMapper.GetMatchingManufacturer(magentoProduct.custom_attributes);

			var productMapping = _fieldMapper.GetProductMapping(magentoProduct);

			//Upsert product
			var productDocumentId = _productMapper.UpsertMasterProduct(productMapping, eaCategoryId, ConfigReader.EaClassificationTreeId,
				eaManufacturerId, fields, assets);

			var slug = _productMapper.CalculateSlug(productDocumentId, null);
        
			if (productMapping == null)
			{
                //Add to your library
                productMapping = _productMapper.AddProductToEndlessAisle(slug);

				//Set the mapping on the product so we know it is mapped
				_fieldMapper.CreateMappingForProduct(magentoProduct, productMapping);
			}

		    _pricingMapper.UpsertPricingForCatalogItem(productMapping, magentoProduct.price);

		    //Create availability for product at company level
		    _availabilityMapper.UpsertAvailabilityForCatalogItem(productMapping, _productMapper.GetQuantityBySku(magentoProduct.sku));

			//If we have colors defined, add it as a color definition (if its not already added)
			var colorId = _colorMapper.GetMagentoColorAttribute(magentoProduct);

			if (colorId != null)
			{
				_colorMapper.UpsertColorDefinitions(productDocumentId, int.Parse(colorId));
			}

			return productDocumentId;
		}

		/// <summary>
		/// This function provides an example of how to create a product structure in EA given a Magento SKU
		/// This function specifically creates a Master Product with Variations from a *CONFIGURABLE* Magento Product
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
				var assets = _assetMapper.ParseAssetsFromProduct(childProductObj, ConfigReader.MagentoStorageConfiguration);

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

				if (productMapping == null)
				{
                    //Add to Endless Aisle
                    productMapping = _productMapper.AddProductToEndlessAisle(slug);

					//Set the mapping on the product to make updating it easier next time
					_fieldMapper.CreateMappingForProduct(childProduct, productMapping);
				}

				_pricingMapper.UpsertPricingForCatalogItem(productMapping, magentoProduct.price);

				//Create availability for product at company level
				_availabilityMapper.UpsertAvailabilityForCatalogItem(productMapping, _productMapper.GetQuantityBySku(magentoProduct.sku));
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

			var magentoCredentials = new AuthenticationCredentialsResource
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

			var eaCredentials = new AuthenticationCredentialsResource()
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
