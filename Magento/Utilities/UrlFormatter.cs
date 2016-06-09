using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MagentoSync.Utilities
{
	public class UrlFormatter
	{
		public readonly string Environment;
		public readonly string MagentoUrl;
		public readonly int CompanyId;
		public readonly int LocationId;

		public readonly string EaProductLibraryTemplate = "https://productlibrary{UrlSuffix}.iqmetrix.net/v1";
		public readonly string EaCatalogsTemplate = "https://catalogs{UrlSuffix}.iqmetrix.net/v1";
		public readonly string EaAuthBaseTemplate = "https://accounts{UrlSuffix}.iqmetrix.net/v1";
		public readonly string EaEntitiesBaseTemplate = "https://entitymanager{UrlSuffix}.iqmetrix.net/v1";
		public readonly string EaAssetsTemplate = "https://ams{UrlSuffix}.iqmetrix.net";
		public readonly string EaAvailabilityTemplate = "https://availability{UrlSuffix}.iqmetrix.net/v1";
		public readonly string EaProductManagerTemplate = "https://productlibrary{UrlSuffix}.iqmetrix.net/ProductManager";
		public readonly string EaPricingTemplate = "https://pricing{UrlSuffix}.iqmetrix.net/v1";
		public readonly string EaOrderTemplate = "https://order{UrlSuffix}.iqmetrix.net/v1";


		public readonly string EaProductLibraryUrl;
		public readonly string EaCatalogsUrl;
		public readonly string EaAuthUrl;
		public readonly string EaEntitiesUrl;
		public readonly string EaAssetsUrl;
		public readonly string EaAvailabilityUrl;
		public readonly string EaProductManagerUrl;
		public readonly string EaPricingUrl;
		public readonly string EaOrderUrl;

		private const string HypermediaFilterQuery = "?$filter=";
		private const string HypermediaAndSeparator = "and";

		public UrlFormatter ()
		{
			//Read in values from App.config
			Environment = ConfigReader.EaEnviornment;
			MagentoUrl = ConfigReader.MagentoUrl;
			CompanyId = ConfigReader.EaCompanyId;
			LocationId = ConfigReader.EaLocationId;

			//Replace {UrlSuffix} with enviornment, for applicable endpoints
			EaProductLibraryUrl = ReplaceEnviornment(EaProductLibraryTemplate);
			EaCatalogsUrl = ReplaceEnviornment(EaCatalogsTemplate);
			EaAuthUrl = ReplaceEnviornment(EaAuthBaseTemplate);
			EaEntitiesUrl = ReplaceEnviornment(EaEntitiesBaseTemplate);
			EaAssetsUrl = ReplaceEnviornment(EaAssetsTemplate);
			EaAvailabilityUrl = ReplaceEnviornment(EaAvailabilityTemplate);
			EaProductManagerUrl = ReplaceEnviornment(EaProductManagerTemplate);
			EaPricingUrl = ReplaceEnviornment(EaPricingTemplate);
			EaOrderUrl = ReplaceEnviornment(EaOrderTemplate);
		}

		/**
		 * Replaces UrlSuffix with the appropriate Enviornment suffix
		 * 
		 * @param   url     Url template     
		 * @return  string  Usable URL
		 */
		private string ReplaceEnviornment(string url)
		{
			return url.Replace("{UrlSuffix}", Environment);
		}

	#region Magento URLs
		/**
		 * Gets location of Magento assets
		 * 
		 * @param   serverPath  Path to Magento files 
		 *  
		 * @return  string      Server path string
		 */
		public string MagentoCatalogAssetPath(string serverPath)
		{
			return string.Format("{0}/pub/media/catalog/product", serverPath);
		}

		/**
		 * @return  string  URL needed to authenticate with Magento
		 */
		public string MagentoAuthUrl()
		{
			return string.Format("{0}/index.php/rest/V1/integration/admin/token", MagentoUrl);
		}

		/**
		 * @param   productSku   SKU for a Product
		 * 
		 * @return  string       URL needed to get products by sku in Magento
		 */
		public string MagentoGetProductBySkuUrl(string productSku)
		{
			return string.Format("{0}rest/V1/products/{1}", MagentoUrl, productSku);
		}

		/**
		 * @return  string  URL needed to create products in Magento
		 */
		public string MagentoCreateProductUrl()
		{
			return string.Format("{0}rest/V1/products", MagentoUrl);
		}

		/**
		 * @return  string  URL needed to get custom attribute details in Magento
		 */
		public string MagentoCustomAttributeUrl(string attributeCode)
		{
			return string.Format("{0}rest/V1/products/attributes/{1}", MagentoUrl, attributeCode);
		}

		/**
		 * @return  string  URL needed to create custom attributes in Magento
		 */
		public string MagentoCreateCustomAttributeUrl()
		{
			return string.Format("{0}rest/V1/products/attributes", MagentoUrl);
		}

		/**
		 * @return  string  URL needed to search custom attributes defined in Magento
		 */
		public string MagentoSearchCustomAttributesUrl(string propertyName, string attrValue)
		{
			return string.Format("{0}rest/V1/products/attributes?searchCriteria[filter_groups][0][filters][0][field]={1}&searchCriteria[filter_groups][0][filters][0][value]={2}", MagentoUrl, propertyName, attrValue);
		}

		/**
		 * @return  string  URL needed to get attributes in an attribute set in Magento
		 */
		public string MagentoAttributeSetAttributesUrl(int attributeSetId)
		{
			return string.Format("{0}rest/V1/products/attribute-sets/{1}/attributes", MagentoUrl, attributeSetId);
		}

		/**
		 * @return  string  URL needed to get category details in Magento
		 */
		public string MagentoCategoryUrl(int categoryId)
		{
			return string.Format("{0}rest/V1/categories/{1}", MagentoUrl,categoryId);
		}


		/**
		 * @param   productSku   SKU for a Product
		 *
		 * @return  string       URL needed to get configurable product details in Magento
		 */
		public string MagentoConfigurableProductUrl(string productSku)
		{
			return string.Format("{0}rest/V1/configurable-products/{1}/children", MagentoUrl, productSku);
		}

		/**
		 * @param   property    Property to search by
		 * @param   value       Value to search for
		 * @param   condition   Condition. See http://devdocs.magento.com/guides/v2.0/get-started/usage.html for a list of acceptable values
		 *
		 * @return  string  Url needed to search for products in Magento
		 */
		public string MagentoSearchProductsUrl(string property, string value, string condition)
		{
			return string.Format("{0}rest/V1/products?searchCriteria[filter_groups][0][filters][0][field]={1}&searchCriteria[filter_groups][0][filters][0][value]={2}&searchCriteria[filter_groups][0][filters][0][condition_type]={3}", MagentoUrl, property, value, condition);
		}

		/**
		 * @param   sku         SKU of Magento Product
		 * @param   attrCode    Attribute code
		 *
		 * @return  string      Url needed to add custom attributes to products in Magento
		 */
		public string MagentoAddCustomAttributeUrl(string sku, string attrCode)
		{
			return string.Format("{0}rest/V1/products/{1}/attributes/{2}", MagentoUrl, sku, attrCode);
		}

		/// <summary>
		/// Gets the URL needed to see inventory details for products in Magento by searching via SKU
		/// </summary>
		/// <param name="sku">SKU to get product for</param>
		/// <returns>URL needed to see inventory details for products in Magento by searching via SKU</returns>
		public string MagentoInventoryBySkuUrl(string sku)
		{
			return string.Format("{0}rest/V1/stockItems/{1}", MagentoUrl, sku);
		}

		/// <summary>
		/// Returns the URL for getting a Magento customer associated with the customer ID provided
		/// </summary>
		/// <param name="customerId">Customer ID to search for</param>
		/// <returns>URL for getting Magento customer</returns>
		public string MagentoGetCustomerByIdUrl(int customerId)
		{
			return string.Format("{0}rest/V1/customers/{1}", MagentoUrl, customerId);
		}

		/// <summary>
		/// Returns the URL for making a Magento customer a cart
		/// </summary>
		/// <param name="customerId">Customer ID to create cart for</param>
		/// <returns>URL for creating Magento customer's cart</returns>
		public string MagentoCreateCartUrl(int customerId)
		{
			return string.Format("{0}rest/V1/customers/{1}/carts", MagentoUrl, customerId);
		}
		
		/// <summary>
		/// Returns the URL for adding a products to a cart
		/// </summary>
		/// <param name="cartId">Cart to add items to</param>
		/// <returns>URL for adding a products to a cart</returns>
		public string MagentoAddItemToCartUrl(int cartId)
		{
			return string.Format("{0}rest/V1/carts/{1}/items", MagentoUrl, cartId);
		}

		/// <summary>
		/// Returns the URL for getting a cart's shipping methods
		/// </summary>
		/// <param name="cartId">Cart to get shipping methods for</param>
		/// <returns>URL for getting a cart's shipping methods</returns>
		public string MagentoGetShippingMethodsForCartUrl(int cartId)
		{
			return string.Format("{0}rest/V1/carts/{1}/shipping-methods", MagentoUrl, cartId);
		}

		/// <summary>
		/// Returns the URL for getting Magento Countries
		/// </summary>
		/// <returns>URL for getting Magento Countries</returns>
		public string MagentoGetCountriesUrl()
		{
			return string.Format("{0}rest/V1/directory/countries", MagentoUrl);
		}

		/// <summary>
		/// Returns the URL for setting a cart's shipping and billing information
		/// </summary>
		/// <param name="cartId">Cart to set information for</param>
		/// <returns>URL for setting a cart's shipping and billing information</returns>
		public string MagentoSetShippingInformationUrl(int cartId)
		{
			return string.Format("{0}rest/V1/carts/{1}/shipping-information", MagentoUrl, cartId);
		}

		/// <summary>
		/// Returns the URL for getting a cart's payment methods
		/// </summary>
		/// <param name="cartId">Cart to get payment methods for</param>
		/// <returns>URL for getting a cart's payment methods</returns>
		public string MagentoGetPaymentMethodsUrl(int cartId)
		{
			return string.Format("{0}rest/V1/carts/{1}/payment-methods", MagentoUrl, cartId);
		}

		/// <summary>
		/// Returns the URL for adding a payment method to a cart
		/// </summary>
		/// <param name="cartId">Cart to add payment method to</param>
		/// <returns>URL for adding a payment method to a cart</returns>
		public string MagentoAddPaymentMethodUrl(int cartId)
		{
			return string.Format("{0}rest/V1/carts/{1}/selected-payment-method", MagentoUrl, cartId);
		}

		/// <summary>
		/// Returns the URL for creating an order for a cart
		/// </summary>
		/// <param name="cartId">Cart to create order for</param>
		/// <returns>URL for creating an order for a cart</returns>
		public string MagentoCreateAnOrderUrl(int cartId)
		{
			return string.Format("{0}rest/V1/carts/{1}/order", MagentoUrl, cartId);
		}

		/// <summary>
		/// Returns the URL getting a cart
		/// </summary>
		/// <param name="cartId">Cart to get</param>
		/// <returns>URL getting a cart</returns>
		public string MagentoGetCartUrl(int cartId)
		{
			return string.Format("{0}rest/V1/carts/{1}", MagentoUrl, cartId);
		}

		/// <summary>
		/// Returns the URL for getting a cart's items
		/// </summary>
		/// <param name="cartId">Cart to get items for</param>
		/// <returns>URL for getting a cart's items</returns>
		public string MagentoGetItemsInCartUrl(int cartId)
		{
			return string.Format("{0}rest/V1/carts/{1}/items", MagentoUrl, cartId);
		}
	#endregion

	#region EA URLs
		
		 /**
		 * @return  string  Url needed to authenticate with EA
		 */
		public string EndlessAisleAuthUrl()
		{
			return string.Format("{0}/oauth2", EaAuthUrl);
		}

		/**
		 * @param   catalogItemId   Identifier of a CatalogItem
		 * 
		 * @return  string          URL needed GET CatalogItems in EA
		 */
		public string EndlessAisleGetCatalogUrl(string catalogItemId)
		{
			return string.Format("{0}/Companies({1})/catalog/items({2})", EaCatalogsUrl, CompanyId, catalogItemId);
		}

		/**
		 * @return  string  URL needed CREATE CatalogItems in EA
		 */
		public string EndlessAisleCreateCatalogUrl()
		{
			return string.Format("{0}/Companies({1})/catalog/items", EaCatalogsUrl, CompanyId);
		}

		public string EndlessAisleGetCatalogItemsBySlugUrl(string slug)
		{
			if(!Regex.IsMatch(slug, RegexPatterns.SlugPattern))
				throw new Exception("Invalid slug format provided.");

			return string.Format("{0}/companies({1})/catalog/items(Slug={2})", EaCatalogsUrl, CompanyId, slug);
		}

		/**
		 * @return  string  URL needed to create products in EA
		 */
		public string EndlessAisleCreateProductUrl()
		{
			return string.Format("{0}/ProductDocs", EaProductLibraryUrl);
		}

		/**
		 * @return  string  URL needed to read classification trees in EA
		 */
		public string EndlessAisleClassificationTreesUrl()
		{
			return string.Format("{0}/ClassificationTrees", EaProductLibraryUrl);
		}

		/**
		 * @param   classificationTreeId    Identifier of a ClassificationTree
		 * 
		 * @return  string                  URL needed GET a classification tree
		 */
		public string EndlessAisleClassificationTreeUrl(int classificationTreeId)
		{
			return string.Format("{0}/ClassificationTrees({1})", EaProductLibraryUrl, classificationTreeId);
		}

		/**
		 * @return  string  URL needed to get field definitions
		 */
		public string EndlessAisleFieldDefinitionsUrl()
		{
			return string.Format("{0}/FieldDefinitions", EaProductLibraryUrl);
		}

		/**
		 * @param   fieldDefinitionId    Identifier of a FieldDefinition
		 * 
		 * @return  string               URL needed GET a field definition
		 */
		public string EndlessAisleFieldDefinitionUrl(int fieldDefinitionId)
		{
			return string.Format("{0}/FieldDefinitions({1})", EaProductLibraryUrl, fieldDefinitionId);
		}

		/**
		 * @return  string  URL needed to get manufacturers
		 */
		public string EndlessAisleEntitiesManufacturersUrl()
		{
			return string.Format("{0}/Manufacturers", EaEntitiesUrl);
		}

		/**
		 * @return  string  URL needed to get a manufacturer
		 */
		public string EndlessAisleEntitiesManufacturerUrl(int manufacturerId)
		{
			return string.Format("{0}/Manufacturers({1})", EaEntitiesUrl, manufacturerId);
		}

		/**
		 * @return  string  URL needed to create variations
		 */
		public string EndlessAisleCreateVariationUrl(int productDocumentId)
		{
			return string.Format("{0}/ProductDocs({1})/variations", EaProductLibraryUrl, productDocumentId);
		}

		/**
		 * @return  string  URL needed to update variations
		 */
		public string EndlessAisleUpdateVariationUrl(int productDocumentId, int variationId)
		{
			return string.Format("{0}/ProductDocs({1})/variations?variationId={2}", EaProductLibraryUrl, productDocumentId, variationId);
		}   

		/**
		 * @return  string  URL needed to get and modify product documents
		 */
		public string EndlessAisleGetProductUrl(int productDocumentId)
		{
			return string.Format("{0}/ProductDocs({1})", EaProductLibraryUrl, productDocumentId);
		}

		/**
		 * @param   slug    Identifier for a product in EA
		 * 
		 * @return  string  URL needed to get products in EA
		 */
		public string EndlessAisleGetProductBySlugUrl(string slug)
		{
			return string.Format("{0}/Products/{1}", EaProductLibraryUrl, slug);
		}

		/**
		 * @param   productDocument     Identifier for a product in EA
		 * 
		 * @return  string              URL needed to add color definitions in EA
		 */
		public string EndlessAisleCreateColorDefinition(int productDocumentId)
		{
			return string.Format("{0}/ProductDocs({1})/colordefinitions", EaProductLibraryUrl, productDocumentId);
		}

		/**
		 * @return  string  URL needed to get color tags in EA
		 */
		public string EndlessAisleColorTagsUrl()
		{
			return string.Format("{0}/ColorTags", EaProductLibraryUrl);
		}

		/**
		 * @return  string  URL needed to create assets in EA
		 */
		public string EndlessAisleCreateAssetUrl()
		{
			return string.Format("{0}/assets", EaAssetsUrl);
		}

		/**
		 * @return  string  URL needed to get assets in EA
		 */
		public string EndlessAisleGetAssetUrl(string assetId)
		{
			return string.Format("{0}/assets/{1}", EaAssetsUrl, assetId);
		}

		/**
		 * @return  string  URL needed to get assets in EA
		 */
		public string EndlessAisleCreateAvailabilityUrl()
		{
			return string.Format("{0}/companies({1})/catalogitems", EaAvailabilityUrl, CompanyId);
		}     
   
		/**
		 * @param   slug    Identifier of a product in EA
		 * 
		 * @return  string  URL needed to set a heroshot in EA
		 */
		 public string EndlessAisleSetHeroShotUrl(string slug)
		{
			return string.Format("{0}/products/{1}/heroshot", EaProductManagerUrl, slug);      
		}

		/// <summary>
		/// Gets the string for the URL needed to set pricing in EA
		/// </summary>
		/// <returns>string for the URL needed to set pricing in EA</returns>
		public string EndlessAisleCreatePricingUrl()
		{
			return string.Format("{0}/companies({1})/Pricing", EaPricingUrl, CompanyId);
		}

		/// <summary>
		/// Returns the URL for getting company's orders in EA
		/// </summary>
		/// <returns>URL for getting company's orders in EA</returns>
		public string EndlessAisleGetOrdersUrl()
		{
			return string.Format("{0}/Companies({1})/Orders", EaOrderUrl, CompanyId);
		}

		/// <summary>
		/// Returns the URL for getting company's orders in EA
		/// </summary>
		/// <returns>URL for getting company's orders in EA</returns>
		public string EndlessAisleGetOrdersByTimeUrl()
		{
			return string.Format("{0}/Companies({1})/Orders", EaOrderUrl, CompanyId);
		}

		/// <summary>
		/// Returns the URL for getting an order's items in EA
		/// </summary>
		/// <param name="orderId">Order to get items for</param>
		/// <returns>URL for getting an order's items in EA</returns>
		public string EndlessAisleGetOrderItemsUrl(string orderId)
		{
			return string.Format("{0}/Companies({1})/Orders({2})/Items", EaOrderUrl, CompanyId, orderId);
		}

		/// <summary>
		/// Returns the URL for getting a catalog item in EA
		/// </summary>
		/// <param name="catalogItemId">Catalog item to get</param>
		/// <returns>URL for getting a catalog item in EA</returns>
		public string EndlessAisleGetCatalogItemUrl(string catalogItemId)
		{
			return string.Format("{0}/companies({1})/catalog/items({2})", EaCatalogsUrl, CompanyId, catalogItemId);
		}

		/// <summary>
		/// Returns the URL for getting the location in EA
		/// </summary>
		/// <returns>URL for getting the location in EA</returns>
		public string EndlessAisleGetLocationUrl()
		{
			return string.Format("{0}/Companies({1})/Locations({2})", EaEntitiesUrl, CompanyId, LocationId);
		}
		#endregion

		/// <summary>
		/// Appends the filters to the URL passed in.
		/// 
		/// ex. http://some.api.com/Location?$filter=City eq 'Berlin' and Created ge datetime'2014-06-21' and Created le datetime'2014-09-22'
		/// </summary>
		/// <param name="url">Base URL to append to</param>
		/// <param name="filters">Filters to add to request</param>
		/// <returns>Base URL with filters appended</returns>
		public string HypermediaFilterUrl(string url, params Filter[] filters)
		{
			if (filters.Length == 0)
				return url;

			StringBuilder result = new StringBuilder(url);
			result.Append(HypermediaFilterQuery);

			var filterList = filters.ToList();

			//Append the first filter as is. Successive filters need to be delimited by a separator
			result.Append(filterList.First());
			filterList.RemoveAt(0);

			foreach (var filter in filterList)
			{
				result.Append(string.Format(" {0} {1}", HypermediaAndSeparator, filter));	//Separator and filter
			}
			return result.ToString();
		}
	}
}
