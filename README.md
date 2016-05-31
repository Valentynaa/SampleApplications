# MagentoConnect

This project is intended to accompany the [Endless Aisle Integration Guide](http://developers.iqmetrix.com/guides/ea-guide/) to provide an example how to integrate the iQmetrix [Endless Aisle](http://www.iqmetrix.com/products/endless-aisle) solution with [Magento](https://magento.com/) utilizing APIs.

Specifically, this project will demonstrate how to:
* Add a Product to Endless Aisle when a Product is created in Magento
* Update a Product in Endless Aisle when a Product is modified in Magento
* Add an Order to Magento when it is created in Endless Aisle

## Table Of Contents

* [Important Notes](#important-notes)
* [Getting Started](#getting-started)
* [How it Works](#how-it-works)
* [Limitations](#limitations)
	* [Required Product Fields](#required-product-fields)
	* [Magento File Storage](#magento-file-storage)
	* [Supported Product Types](#supported-product-types)
	* [Supported Attribute Types](#supported-attribute-types)
	* [SKUs/UPCs](#skusupcs)
	* [Removing Variations](#removing-variations)
	* [Asset Management](#asset-management)
	* [Color](#colors)
	* [Localization](#localization)
	* [Units](#units)
* [File Structure](#file-structure)
	* [Controllers](#controllers)
	* [Models](#models)
	* [Utilities](#utilities)
	* [App.config](#appconfig)
* [Tests](#tests)
* [Logging](#logging)

## Important Notes

* This project uses the [Magento REST API](http://devdocs.magento.com/guides/v2.0/rest/bk-rest.html) using `JSON`. SOAP and XML are **not** covered in the scope of this project
* Comments follow the [Javadoc](http://www.oracle.com/technetwork/articles/java/index-137868.html) standard

## Getting Started

See [Getting Started Guide](https://github.com/iQmetrix/MagentoConnect/blob/master/docs/GettingStarted.md)

## How it Works

The App works according to the following logic:
* Product Sync
    * All products created or updated in Magento since the last sync, or the last **hour** (this value can be changed) if no sync data is found, are fetched
    * Each product is checked for the `{MappingCode}` attribute
    * If the product is mapped, an update is performed. If the product is **not** mapped, a create is performed
    * The product is checked for type (simple/virtual/configurable)
        * Configurable products are created as Master Products with Variations in EA
        * Simple or Virtual products are created as Master Products in EA
    * Each field specified in `{FieldMapping}` is created or updated
    * Each image is checked, if there are new images they are uploaded to EA 
    * The Magento **base** image is set to the product's **hero shot** image
    * The product is checked for a valid mapped category and manufacturer
    * The Product is created or updated in EA
    * For NEW products
        * A [CatalogItem](http://developers.iqmetrix.com/api/catalog/#catalogitem) is created
        * [Inventory Availability](http://developers.iqmetrix.com/api/availability/#availability) is set (non serialized, Quantity of 1) at `{EA_LocationId}` 
        * The [Slug](http://developers.iqmetrix.com/api/catalog/#product-slug) of the new product is calculated
        * The Slug is added to the magento product in the `{MappingCode}` attribute
    * If the Magento product has a color, it is created or updated as a ColorDefinition on the product
    * If the Magento product has a quantity, it is created or updated as an [Availability](/api/availability/#availability) resource
    * If the Magento product has a price, it is created or updated as a [Pricing](/api/pricing/#pricing) resource
* Order Sync (after product sync)
    * Each [Order](http://developers.iqmetrix.com/api/orders/#order) created in Endless Aisle since the last sync, or the last **hour** (this value can be changed) if no sync data is found, are fetched
    * For each order, a cart is created in Magento for `{Magento_CustomerId}` specified in the config file
    * For each [OrderItem](http://developers.iqmetrix.com/api/orders/#item), the corresponding [CatalogItem](http://developers.iqmetrix.com/api/catalog/#catalogitem) is found
    * From the [CatalogItem](http://developers.iqmetrix.com/api/catalog/#catalogitem) the Magento product with the same `{MappingCode}` is added to the cart
    * The `{Magento_ShippingCode}` and other shipping information is set for the cart are then set based on the `{EA_LocationId}`
    * The `{Magento_PaymentMethod}` is then set for the cart
    * An order is then created from the cart

## Limitations

This project is intended to provide an example of an integration, and has many limitations:

### Required Product Fields

To add a Magento product to Endless Aisle, the product must have a:

* Sku
* Name 
* Category, the first category listed will be used if there are multiple listed
* Manufacturer
* Image

### Magento File Storage

In Magento there are two ways to store images. The only method supported in the app is **File System**, the default method.

To change this value:

* Log into Magento admin
* Click on **Stores** in sidebar, then **Configuration** under Settings 
* Click on **Advanced** in sidebar, then **System**
* Maximize **Storage Configuration For Media** if it is minimized
* Change **Media Storage** to **File System**

### Supported Product Types

The following Magento product types are supported: **Simple**, **Configurable** and **Virtual**.

Simple and Virtual products will be created as [Master Products](http://developers.iqmetrix.com/concepts/product-structure/#master-products) in EA.

Configurable Products will be created as a Master Product with [Variations](http://developers.iqmetrix.com/concepts/product-structure/#variations) for each configured child in EA.

[Revisions](http://developers.iqmetrix.com/concepts/product-structure/#revisions) are not supported.

### Supported Attribute Types

The following Magento attribute types are supported: **text**, **textarea** and **multiselect** and **select**.

### SKUs/UPCs

Products in Endless Aisle may have several assigned Identifiers, see [Identifier Groups](http://developers.iqmetrix.com/api/product-structure/#identifiergroup).

The App does not include functionality to sync to these values.

### Removing Variations

The App **cannot** remove a Variation from a Master Product once it is assigned.

In Magento, if a configurable product is synced including child products, these products will be created as Variations on a Master Product.

If a child product is later removed, this change will **not** be synced to EA.

### Asset Management

The App can only **add new assets** to Products, **not remove or change** existing assets.

In Magento, if you have a Product that is synced to EA and change or remove images from the product, these changes will **not** be synced to EA.

### Colors

Products in Endless Aisle can have [ColorDefinitions](http://developers.iqmetrix.com/api/catalog/#colordefinition) with Swatches. 

The App **cannot** create Swatches or remove or update ColorDefinitions once they are created.

If the App encounters a Magento product with an unmapped Color, it will ignore the value and return a console message to that effect.

### Localization

Localization is not supported, a language pack would be required to implement this feature.

### Units

Base Magento is unit agnostic, aside from Currency and Weight, which are set globally.

Product Library supports units for fields.

Units are not supported in the App, a plugin would be required to implement this feature.

### Prices and Quantities

Base Magento does not allow you to set price or quantity at a store level, unlike Endless Aisle.

When MagentoConnect syncs price and quantity, it sets these values at the Company level in Endless Aisle.

Using a Magento extention could allow you to set price and quantity at the Location level in Endless Aisle.

## File Structure

The files in the Magento Connect project are organized in the following structure:

* Controllers - Classes that deal specifically with sending API requests
* Mappers - Classes with business logic for the app
* Models - Transport objects, or classes representing resources
* Properties - Visual Studio project information
* Utilities - Useful utility classes
* .gitignore - Git ignore file, see [Git Ignore](https://git-scm.com/docs/gitignore)
* App.config - Configuration file to hold mapping values, constants and sensitive information such as authentication credentials
* App.cs - Main console application file
* MagentoConnect.csproj - Visual Studio project file
* MagentoConnect.sln - Visual Studio solution file
* packages.config - Packages file describing nuget packages, see [Nuget](https://www.nuget.org/)

### Controllers

This folder contains controller classes which manage the API requests.
Controllers are organized by Magento/Endless Aisle and API.

Base:
* **BaseController** - All controllers extend this class, which contains the UrlFormatter and all future utility classes

EndlessAisle:
* **AssetsController** - Allows you to create an Asset, see [Assets](http://developers.iqmetrix.com/api/assets/)
* **AuthController** - Allows you to get an Auth Token, needed to call EA APIs
* **AvailabilityController** - Allows you to create inventory availability, see [Inventory Availability](http://developers.iqmetrix.com/api/availability/)
* **CatalogsController** - Allows you to manage your Catalog, see [Catalog](http://developers.iqmetrix.com/api/catalog/)
* **ClassificationController** - Controller used by Tests, see [ClassificationTree](http://developers.iqmetrix.com/api/classification-tree/)
* **EntitiesController** - Allows you to get Manufacturer and Location information, see [Entities](http://developers.iqmetrix.com/api/entity-store/)
* **FieldDefinitionController** - Allows you to get Field Definitions, see [FieldDefinitions](http://developers.iqmetrix.com/api/field-definitions/)
* **OrdersController** - Allows you to get Order information, see [Orders](http://developers.iqmetrix.com/api/orders/)
* **PricingController** - Allows you to set Prices, see [Pricing](http://developers.iqmetrix.com/api/pricing/)
* **ProductLibraryController** - Allows you to create Master Products, Variations and Revisions, see [Product Structure ](http://developers.iqmetrix.com/api/product-structure/)

Magento:
* **AuthController** - Allows you to get an Auth Token, needed to call Magento APIs
* **CartController** - Allows you to create, manage, and create an Order for a Cart
* **CategoryController** - Allows you to get details about Categories
* **CustomAttributesController** - Allows you to get details about Custom Attributes on a Product
* **CustomerController** - Allows you to get details about a Customer
* **ProductController** - Allows you to get Magento products by SKU
* **RegionController** - Allows you to get Country and Region information for Magento

### Models

This folder contains Transport Objects organized by Magento/Endless Aisle and API.

Transport objects are used in this project by [RestSharp](http://restsharp.org/) and [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json/) which serialize responses from API requests directly into objects.

The table below lists each folder and the relevant API reference.

| Folder | SubFolder | Relevant API Reference |
|:-------|:----------|:-----------------------|
| EndlessAisle | | |
| | Assets | [Assets](http://developers.iqmetrix.com/api/assets/) |
| | Authentication | [Authentication](http://developers.iqmetrix.com/api/authentication/) |
| | Availability | [Inventory Availability](http://developers.iqmetrix.com/api/availability/) | 
| | Catalog | [Catalog](http://developers.iqmetrix.com/api/catalog/) |
| | ClassificationTree | [Classification Tree](http://developers.iqmetrix.com/api/classification-tree/) |
| | Entities | [Entities](http://developers.iqmetrix.com/api/entity-store/) |
| | FieldDefinitions | [FieldDefinitions](http://developers.iqmetrix.com/api/field-definitions/) |
| | Orders | [Orders](http://developers.iqmetrix.com/api/orders/) |
| | Pricing | [Pricing](http://developers.iqmetrix.com/api/pricing/) |
| | ProductLibrary | [Product Structure](http://developers.iqmetrix.com/api/product-structure/) |
| Magento | | |
| | Authentication | [Magento REST API Reference](http://devdocs.magento.com/swagger/index.html#/) |
| | Cart | [Magento REST API Reference](http://devdocs.magento.com/swagger/index.html#/) |
| | Category | [Magento REST API Reference](http://devdocs.magento.com/swagger/index.html#/)  |
| | Country | [Magento REST API Reference](http://devdocs.magento.com/swagger/index.html#/) |
| | CustomAttributes | [Magento REST API Reference](http://devdocs.magento.com/swagger/index.html#/)  |
| | Customer | [Magento REST API Reference](http://devdocs.magento.com/swagger/index.html#/) |
| | Inventory | [Magento REST API Reference](http://devdocs.magento.com/swagger/index.html#/) |
| | Products | [Magento REST API Reference](http://devdocs.magento.com/swagger/index.html#/)  | 
| Mapping | | |
| | MagentoEaMapping | |

### Utilities

The following utilities are included in the project,

* **ConfigReager** - Reads values from App.config, used for reading authentication information and Environment value
* **LogUtility.cs** - Used for operations involved with logging such as writing logs or retrieving log data
* **RegexPatterns** - Holds Regex patterns used for validation
* **UrlFormatter** - Formats and returns endpoint URLs as strings given URL variables. This is useful because Endless Aisle endpoints differ between [Enviornments](http://developers.iqmetrix.com/api/environments/). 
* **Filter** - Allows the ability to filter results from services built on the Hypermedia API framework such as Orders, and Pricing

### App.config

The following constants must be filled out in the App.config file for the project to run successfully:

* `EA_Environment` - This should be always `demo` to avoid corrupting production data
* `EA_GrantType` - This should always be `password` unless you are changing the authentication method 
* `EA_ClientId` - Client Id, provided in your onboarding package from iQmetrix
* `EA_Username` - Username, provided in your onboarding package from iQmetrix
* `EA_Password` - Password, provided in your onboarding package from iQmetrix
* `EA_ClientSecret` - Client secret, provided in your onboarding package from iQmetrix
* `EA_CompanyId` - CompanyId, provided in your onboarding package from iQmetrix
* `EA_LocationId` - LocationId, provided in your onboarding package from iQmetrix. This should be the Location your Endless Aisle is configured for.
* `EA_ClassificationTreeId` - ClassificationTreeId, provided in your onboarding package from iQmetrix
* `MappingCode` - Attribute code of Mapping property in Magento
* `Magento_Url` - Location of your Magento server
* `Magento_Username` - The username you use to log into the Magento admin portal
* `Magento_Password` - The password you use to log into the Magento admin portal
* `Magento_ServerPath` - Location of Magento on your server

The following values are constants in Magento and will only need to be changed if your Magento system is highly customized.

* `Magento_ManufacturerCode` - Attribute code for Magento manufacturer property
* `Magento_GreaterThanCondition` - Magento constant used for filtering
* `Magento_EqualsCondition` - Magento constant used for filtering
* `Magento_SearchDateString` - Magento constant for date format when searching
* `Magento_ConfigurableTypeId` - Magento constant for configurable type
* `Magento_CategoryCode` - Magento attribute code for category
* `Magento_DescriptionCode` - Magento attribute code for description
* `Magento_CreatedAtProperty` - Magento attribute code for created at
* `Magento_UpdatedAtProperty` - Magento attribute code for updated at
* `Magento_NameCode` - Magento attribute code for name
* `Magento_ColorCode` - Magento attribute code for color
* `Magento_MaterialCode` - Magento attribute code for material
* `Magento_ImageCode` - Magento attribute code for image
* `Magento_CustomerId` - Magento customer used for creating orders
* `Magento_ShippingCode` - Magento shipping option used for creating orders
* `Magento_PaymentMethod` - Magento payment option used for creating orders

## Tests

The App solution includes a series of unit tests which are intended to help figure out problems with the App.

Many of these files have defined constants. These will need to be replaced with values from your Magento or Endless Aisle system before the tests can be run.

| Test Folder | Uses |
|:------------|:-----|
| Configuration | Diagnose problems related to App.config entries |
| Controllers | Ensure Controllers are working correctly |
| Mappers | Ensure the Mappers are working correctly |
| ProductSync | Diagnose problems with the Product sync |
| Utilities | Determine if there are any problems with the Utility classes |

## Logging

When Magento Connect runs it creates log files in the same location as the executable. Using Visual Studio, this is generally bin/Debug or bin/Release.

Product syncs are logged in `productSyncLog.txt`, order syncs are logged in`orderSyncLog.txt`, errors are logged in `errorLog.txt`.

Additional logging can be added by extending `Enums.cs`.

If you encounter problems, check these logs for additional details.
