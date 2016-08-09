# Shopify Integration
This project is intended to provide an example how to integrate the iQmetrix Platform with [Shopify](https://www.shopify.ca/) utilizing APIs.

Specifically, this project will demonstrate how to:

* Add or update a Product for iQmetrix when a Product is created or modified in Shopify along with its inventory and pricing information
* Add or update a Customer for iQmetrix when a Customer is created or modified in Shopify


## Table Of Contents
* [Getting Started](#getting-started)
	* [FieldMapping](#fieldmapping)
	* [CategoryMapping](#categorymapping)
	* [ManufacturerMapping](#manufacturermapping)
* [How it Works](#how-it-works)
	* [customer/update](#customerupdate)
	* [customer/delete](#customerdelete)
	* [product/create](#productcreate)
	* [product/update](#productupdate)
	* [product/delete](#productdelete)
* [Limitations](#limitations)
	* [Required Product Fields](#required-product-fields)
	* [SKUs/UPCs](#skusupcs)
	* [Removing Variations](#removing-variations)
	* [Localization](#localization)
	* [Metafield Properties](#metafield-properties)
	* [Limited Variations](#limited-variations)
	* [Unsupported Fields](#unsupported-fields)
* [File Structure](#file-structure)
	* [iQmetrix API Node](#iqmetrix-api-node)
	* [Mappers](#mappers)
	* [Routes](#routes)
	* [Utilities](#utilities)
	* [dev-settings.json](#dev-settingsjson)

## Getting Started
To set up the environment necessary for the application please see the [Getting Started Guide](https://github.com/iQmetrix/SampleApplications/blob/master/Shopify/README.md).

Before the App can be run, you must first create a series of Mapping values in [`dev-settings.json`](https://github.com/iQmetrix/SampleApplications/blob/master/Shopify/ShopifyApp/dev-settings.json). Configuration is done in JSON format with key value pairs.

### FieldMapping

**Format**:

```json
"fieldMapping": {
	"{{shopifyProductField}}": {{iqmetrixFieldDefinitionId}}
}
```

For each **field** you wish to sync from Shopify to iQmetrix, there must be a property listed in `fieldMapping`.

* `shopifyProductField` is the name of a property on a Shopify [product](https://help.shopify.com/api/reference/product)
    * Endpoint: `GET /admin/products/{productId}.json`
* `iqmetrixFieldDefinitionId` is the Id of a FieldDefinition of the appropriate property to sync 
    * Endpoint: `GET https://productlibrary{UrlSuffix}.iqmetrix.net/v1/FieldDefinitions`
    * API Reference: [Getting All Field Definitions](http://developers.iqmetrix.com/api/field-definitions/#getting-all-field-definitions)

**Example**:

```json
"fieldMapping": {
	"title": 1
}
```

### CategoryMapping

Category mapping is required. Enter the Shopify [product](https://help.shopify.com/api/reference/product) field that you wish to map as the category to the `shopify_settings.productCategory` field.

For each Category to be mapped, there must be an entry listed in `categoryMapping`.

**NOTE**: Shopify does not directly have a category tree system to organize products so if the category field is a text field the value must **exactly** match the entry in `dev-settings.json` in order to be mapped.


**Format**:
```json
"categoryMapping": {
	"{{shopifyCategoryFieldValue}}": {{iqmetrixClassificationId}}
}
```

For each category in Shopify, there must be an entry listed here.

* `shopifyCategoryFieldValue` is the value of the category field supplied in `shopify_settings.productCategory`.
* `iqmetrixClassificationId` is the Id of a matching Classification (NOT Category)
    * Endpoint: `GET https://productlibrary{{UrlSuffix}}.iqmetrix.net/v1/ClassificationTrees({{ClassificationTreeId}})`
    * API Reference: [Getting a Classification Tree](http://developers.iqmetrix.com/api/classification-tree/#getting-a-classification-tree)

**Example**:

```json
"shopify_settings":{
	"productCategory": "vendor"
},
"categoryMapping": {
	"Shirt": 399
}
```

### ManufacturerMapping

Manufacturer mapping (unlike category) is optional. If you wish to have mapping to an iQmetrix manufacturer enter the Shopify [product](https://help.shopify.com/api/reference/product) field that you wish to map as a manufacturer to the `shopify_settings.productManufacturer` field.

For each Manufacturer to be mapped, there must be an entry listed in `manufacturerMapping`.

**NOTE**: Shopify does not directly have manufacturers on products so if the manufacturer field is a text field the value must **exactly** match the entry in `dev-settings.json` in order to be mapped.


**Format**:
```json
"manufacturerMapping": {
	"{{shopifyManufacturerFieldValue}}": {{iqmetrixManufacturerId}}
}
```

* `shopifyManufacturerFieldValue` is the value of the manufacturer field supplied in `shopify_settings.productManufacturer`.
* `iqmetrixManufacturerId` is the Id of a matching Manufacturer 
    * Endpoint: `GET https://entitymanager{UrlSuffix}.iqmetrix.net/v1/Manufacturers`
    * API Reference: [Getting All Manufacturers](http://developers.iqmetrix.com/api/entity-store/#getting-all-manufacturers)

**Example**:

```json
"shopify_settings":{
	"productManufacturer": "vendor"
},
"manufacturerMapping": {
	"iQmetrix_Test_Store": 10472
}
```

See [Getting Started Guide](https://github.com/iQmetrix/SampleApplications/blob/master/Shopify/README.md) for more details.

## How it Works
The integration uses Shopify [webooks](https://help.shopify.com/api/tutorials/webhooks), which trigger actions in the application. When a webhook request is received the corresponding mapping function is triggered. Below is a description of how the mapping is performed for each supported topic.

### customer/update
* Determines whether or not the customer is already mapped to the iQmetrix Platform
  * If the product does not already exist, the create customer function is called
    1. The [iQmetrix Customer](http://developers.iqmetrix.com/api/crm/#customer) is built and has the customer addresses added to it
	2. The customer is created in iQmetrix with the Shopify customer `id` mapped to its `CustomerExtensions`
	3. The iQmetrix customer `Id` is then mapped to a metafield on the Shopify customer
  * If the customer exists in iQmetrix an update is performed
	1. The iQmetrix customer details are retrieved from the metafield mapping value on the Shopify customer
	2. Any addresses that don't already exist are added to the iQmetrix customer
	3. Any addresses that no longer exist are deleted from the iQmetrix customer
	4. The customer update is performed using the new values.
* **NOTE**: The customer/create topic always (in testing) triggers along with the customer/update topic. This is why customer/create is not a supported topic. If you find that this is not the case, then you can subscribe to the create/customer topic and the application will work in the same flow.
	  
### customer/delete
* Search for customer based on the Shopify customer `id` deleted
* Perform deletion
* **NOTE**: Deleting a customer in iQmetrix sets the `Disabled` field on the customer to `true`. The customer record is not actually deleted.
  
### product/create
* Determines whether or not the product is already mapped to the iQmetrix Platform
  * If the product does not already exist, the create product function is called
	1. The category for the product is determined by the configuration in `dev-settings.json` and the product fields
	2. If the manufacturer is configured to be set, the value is then determined 
	3. All of the assets for the product are created 
	4. The Master Product for the Shopify Product is built using the `fieldMapping` configuration in `dev-settings.json` and the assets from the previous step
	5. Perform the creation for the Master Product
	6. From the Master Product a CatalogItem is created with the Shopify product `id` as the `RmsId`
	7. The iQmetrix `CatalogItemId` is then mapped to a metafield on the Shopify product
  * **NOTE**: Category and Manufacturer can only be set on product creation
	
  * If the product exists in iQmetrix an update is performed
	1. Retrieve the ProductDetails for the CatalogItemId mapped in the Shopify product's metafields
	2. Compare Images on both products to see if new images need to be added to the iQmetrix product
	3. Build Master Product for update using the `fieldMapping` configuration in `dev-settings.json` and the new assets from the previous step
	4. Perform Master Product update
  * **NOTE**: Images can only be addedon product updates
* If the product has multiple variants, ensure each variant has a Variation in iQmetrix
  * A Variation is built from the options on the variant and the `fieldMapping` configuration in `dev-settings.json`
  * Determine whether or not the variant is already mapped to the iQmetrix Platform
  * If the Variation does not already exist, the create Variation function is called
	1. Create Variation in iQmetrix
	2. Create CatalogItem for Variation with `{shopifyProduct.id}-{variant.id}` as the `RmsId`
	3. The iQmetrix `CatalogItemId` is then mapped to a metafield on the variant
  * If the Variation exists in iQmetrix an update is performed
    1. Retrieve the ProductDetails for the CatalogItemId mapped in the variant's metafields
	2. Perform the update for the Variation
* Add Availability for the product
  * If there are multiple variants, create an Availability for the CatalogItem in each variant's metafields
  * If there are not multiple variants, create an Availability for the CatalogItem in the product metafields
* Add Pricing for the product
  * If there are multiple variants, create an Pricing for the CatalogItem in each variant's metafields
  * If there are not multiple variants, create an Pricing for the CatalogItem in the product metafields

### product/update
* See product/create above

### product/delete
* Search for `CatalogItem` based on the Shopify product `id` deleted
* Perform deletion
* **NOTE**: The `CatalogItem` for product is deleted, but the Master Product still exists.

## Limitations
This project is intended to provide an example of an integration, and has many limitations:

### Required Product Fields
To add a Shopify product to the iQmetrix Platform, the product must have a:

* **Product Name** - A field on the Shopify product mapped to the `Product Name` Field Definition in `dev-settings.json` under `fieldMapping`
* **Category** - A field on the Shopify Product to represent the node on the Classification tree that the product should be at. Specify this field in `dev-settings.json` under `shopify_settings.productCategory` and list the valid selections and in `categoryMapping` 

### SKUs/UPCs
Products in the iQmetrix Platform may have several assigned Identifiers, see [Identifier Groups](http://developers.iqmetrix.com/api/product-structure/#identifiergroup).

The App does not include functionality to sync to these values.

### Removing Variations
The App **cannot** remove a Variation from a Master Product once it is assigned.

In Shopify, if a product is synced and has variants, these products will be created as Variations on a Master Product.

If a variant is later removed, this change will **not** be synced to iQmetrix.

### Localization
Localization is not supported.

### Metafield Properties
[Metafields](https://help.shopify.com/api/reference/metafield) created for mapping all have `"global"` as their namespace and `"string"` as their type.
Be aware of this if you are using Shopify metafields for other purposes.

### Limited Variations
What can be done with variations is limited by the behaviour of variants in Shopify. In Shopify, a product can only have 100 variants and each variant can be different on only three fields.

Normally, there could be any number of Variations for a product and any number of fields could differ from the Master Product.

### Unsupported Fields
| Ecosystem | Resource      | Field          | Defaulted Value |
|:----------|:--------------|:---------------|:----------------|
| iQmetrix | [Pricing](http://developers.iqmetrix.com/api/pricing/#pricing)      | PricingTerm    | `null`          |
| iQmetrix | [Pricing](http://developers.iqmetrix.com/api/pricing/#pricing)      | OverridePrice  | `null`          |
| iQmetrix | [Pricing](http://developers.iqmetrix.com/api/pricing/#pricing)      | IsDiscountable | `false`         |
| iQmetrix | [Pricing](http://developers.iqmetrix.com/api/pricing/#pricing)      | FloorPrice     | `null`          |
| iQmetrix | [Availability](http://developers.iqmetrix.com/api/availability/#availability) | IsSerialized   | `false`         |
| iQmetrix | [Customer](http://developers.iqmetrix.com/api/crm/#customer)     | DoNotContact   | `true`          |
| iQmetrix | [Address](http://developers.iqmetrix.com/api/crm/#address)     | DoNotContact   | `true`          |

## File Structure
The files in this project are organized in the following structure:

* **iqmetrix-api-node** - iQmetrix Resources and their callsClasses that deal specifically with sending API requests
* **mappers** - Classes with business logic for the app
* **routes** - Handles API authentication and requests sent to endpoints on the Shopify Store
* **shopify-metafield-node** - Extends the [Shopify API Node](https://github.com/microapps/Shopify-api-node) functionality for metafields
* **utilities** - Useful utility classes
* **app.js** - Initializes server
* **dev-settings.json** - Configuration file to hold mapping values, constants and sensitive information such as authentication credentials

### iQmetrix API Node

The iQmetrix API node is responsible for all calls to the Platform APIs. It contains classes which manage the API requests for different resources and methods for making the calls.

The structure for the iQmetrix API node is based off the design of the [Shopify API node](https://github.com/microapps/Shopify-api-node) package that is used by the application to make calls to the Shopify APIs.

Mixins: 

* **base** - Resource classes extend this base class that makes simple POST, GET, PUT, and DELETE requests.

Resources:

* **asset** - Class that makes requests related to the [Asset](http://developers.iqmetrix.com/api/assets/) resource.
* **availability** - Allows you to create inventory availability, see [Inventory Availability](http://developers.iqmetrix.com/api/availability/)
* **catalog-item** - Class that makes requests related to the [CatalogItem](http://developers.iqmetrix.com/api/catalog/#catalogitem) resource.
* **classification-tree** - Class that makes requests related to the [ClasificationTree](http://developers.iqmetrix.com/api/classification-tree/) resource.
* **customer-address** - Class that makes requests related to the [Address](http://developers.iqmetrix.com/api/crm/#address) resource.
* **customer-contact-method** - Class that makes requests related to the [ContactMethod](http://developers.iqmetrix.com/api/crm/#contactmethod) resource.
* **customer** - Class that makes requests related to the [Customer](http://developers.iqmetrix.com/api/crm/) resource.
* **field-definition** - Class that makes requests related to the [FieldDefinition](http://developers.iqmetrix.com/api/field-definitions/) resource.
* **manufacturer** - Class that makes requests related to the [Manufacturer](http://developers.iqmetrix.com/api/entity-store/#manufacturer) resource.
* **pricing** - Class that makes requests related to the [Pricing](http://developers.iqmetrix.com/api/pricing/) resource.
* **variation** - Class that makes requests related to the [Variation](http://developers.iqmetrix.com/api/product-structure/#variation) resource.

### Mappers 
This folder contains classes that deal with the logic regarding the mapping of objects from Shopify to iQmetrix and vise versa.

Mapping to records in each system is done as shown in the below table:

| Shopify Resource | Shopify Field | iQmetrix Resource | iQmetrix field |
|:-----------------|:--------------|:------------------|:---------------|
|[Customer](https://help.shopify.com/api/reference/customer)|[Metafield](https://help.shopify.com/api/reference/metafield)|[CustomerFull](http://developers.iqmetrix.com/api/crm/#customerfull)|[CustomerExtension](http://developers.iqmetrix.com/api/crm/#customerextension)|
|[Product](https://help.shopify.com/api/reference/product)|[Metafield](https://help.shopify.com/api/reference/metafield)|[CatalogItem](http://developers.iqmetrix.com/api/catalog/#catalogitem)|RmsId|
|[Product Variant](https://help.shopify.com/api/reference/product_variant)|[Metafield](https://help.shopify.com/api/reference/metafield)|[CatalogItem](http://developers.iqmetrix.com/api/catalog/#catalogitem)|RmsId|

### Routes
* **webhook** - Handles webhook requests and passes the required work off to a mapper.
* **iqmetrix_auth.js** - Gets the Auth Token needed to use the iQmetrix APIs
* **shopify_auth.js** - Gets the Auth Token needed to use the Shopify APIs
**NOTE**: This application has no UI, but to add a UI to the application it can be done here. See [index.js renderApp](https://github.com/iQmetrix/SampleApplications/blob/master/Shopify/ShopifyApp/routes/index.js#L40)

### Shopify Metafield Node
Structured similar to [Shopify API node](https://github.com/microapps/Shopify-api-node). Allows the ability to get the metafields from a specific resource.
This was included this because the original package does not support this functionality currently.

### Utilities
The following utilities are included in the project:

* **file** - Downloads files from the web.
* **filter** - Allows the ability to filter results from services built on the Hypermedia API framework such as [Availability](http://developers.iqmetrix.com/api/availability/), [CRM](http://developers.iqmetrix.com/api/crm/), and [Pricing](http://developers.iqmetrix.com/api/pricing/).
* **regex** - Holds Regex patterns used throughout the application.

### dev-settings.json
The following constants must be filled out in the dev-settings.json file for the project to run successfully. For more information see the [Getting Started Guide](https://github.com/iQmetrix/SampleApplications/blob/master/Shopify/README.md):

* `general`
	* `title` - Title for the application
	* `shop_name` - Name of the shop (as it appears in the URL)
	* `language_code` - Language code to use for Accept Language on API calls. Should be `en-US` for English
	* `serve_location` - Folder location of style sheets
	* `log_calls` - Toggles the logging of each API call made in the console
* `oauth`
	* `api_key` - Shopify API key
	* `client_secret` - Client secret for API
	* `redirect_url` - Where to redirect after getting the access code
	* `access_token` - Access token for using Shopify APIs
	* `scope` - Scope of API access
* `shopify_settings`
	* `productCategory` - Field for category mapping (required)
	* `productManufacturer` - Field for manufacturer mapping (optional)
* `iq_oauth`
	* `grant_type` - This should always be `password` unless you are changing the authentication method
	* `client_id` - Client Id, provided in your onboarding package from iQmetrix
	* `client_secret` - Client secret, provided in your onboarding package from iQmetrix
	* `username` - Username, provided in your onboarding package from iQmetrix
	* `password` - Password, provided in your onboarding package from iQmetrix
	* `environment` - This should be always `demo` to avoid corrupting production data
* `iq_settings`
	* `companyId` - CompanyId, provided in your onboarding package from iQmetrix
	* `locationId` - LocationId, provided in your onboarding package from iQmetrix. This should be the Location your Endless Aisle is configured for
	* `mappingFieldName` - Name used in Shopify metafields to identify iQmetrix mapping metafields
	* `addressType` - [Address Type](http://developers.iqmetrix.com/api/crm/#addresstype) for customers
	* `customerType` - [Customer Type](http://developers.iqmetrix.com/api/crm/#customertype) for customers
	* `customerExtensionType` - [Customer Extension Type](http://developers.iqmetrix.com/api/crm/#customerextensiontype) to use for customer mapping
	* `classificationTreeId` - ClassificationTreeId, provided in your onboarding package from iQmetrix
* `fieldMapping`
	* `title` - Should map to the Product Name Field Definition
	* `option1` - Field for Variant option 1
	* `option2` - Field for Variant option 2
	* `option3` - Field for Variant option 3
* `categoryMapping` - See [Getting Started](https://github.com/iQmetrix/SampleApplications/blob/master/Shopify/README.md)
* `manufacturerMapping` - See [Getting Started](https://github.com/iQmetrix/SampleApplications/blob/master/Shopify/README.md)

Example of **dev-settings** setup:
```json
{
	"general": {
		"title": "Shopify App",
		"shop_name": "iqmetrix-test-store",
		"language_code": "en-US",
		"serve_location": "public",
		"log_calls": true
	},
	"oauth": {
		"api_key": "b971d3d325431d320326f34f431a715b",
		"client_secret": "5626afdb60c6f322583f65550dfd0fd9",
		"redirect_url": "http://localhost:3000/auth_token",
		"access_token": "e18c1ba2861394181c3b6b9651bc1b9c",
		"scope": "read_content,write_content,read_products,write_products,read_customers,write_customers,read_orders,write_orders,read_shipping,write_shipping,read_analytics"
	},
	"shopify_settings":{
		"productCategory": "product_type",
		"productManufacturer": "vendor"
	},
	"iq_oauth": {
		"grant_type": "password",
		"client_id": "CLIENTID",
		"client_secret": "df1F1S8vbAE9gs034KFg6ENe",
		"username": "user@name.com",
		"password": "password",
		"environment": "demo"
	},
	"iq_settings": {
		"companyId": 14146,
		"locationId": 14192,
		"mappingFieldName": "iqmetrixidentifier",
		"addressType": "Shipping",
		"customerType": "Person",
		"customerExtensionType": "CorrelationId",
		"classificationTreeId": 395
	},
	"fieldMapping": {
		"title": 1,
		"body_html": 45,
		"option1": 146,
		"option2": null,
		"option3": null
	},
	"categoryMapping": {
		"Shirt": 399
	},
	"manufacturerMapping": {
		"iQmetrix_Test_Store": 10472
	}
}
```
