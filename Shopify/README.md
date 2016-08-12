# Getting Started

This guide goes over everything needed to run the iQmetrix Shopify Integration App.

The intended audience for this guide are developers interested in extending the Shopify Integration App.

---

## Prerequisites

Before checking out this project, ensure you have the following installed on your computer:

* [Node.js](https://nodejs.org/en/)
* [ngrok](https://ngrok.com/) - Secure introspected tunnels to localhost. Required as Shopify webhooks do not support use with localhost for development
* [Postman](https://www.getpostman.com/) - REST Client that allows you to make custom calls

## Knowledge Assumptions

This guide assumes the following:

* **Windows** Operating System
* Familiarity with **Node.js**
* Familiarity with **Git** and **Github**

---

## Setting Up Environment 

These steps must be done in **sequential order**. 

### 1. Set up Shopify
1. Follow the Shopify developer's [getting started](https://help.shopify.com/api/guides) guide to create a development store. When [creating your application](https://help.shopify.com/api/guides/api-credentials) ensure that the app URL is `http://localhost:3000/` and that `http://localhost:3000/auth_token` is the used as the redirection URL since [express](https://expressjs.com/) works for port 3000. For further help take a look at [this blog post](http://blog.codezuki.com/blog/2014/02/10/shopify-nodejs/).

2. Download [this collection](https://www.getpostman.com/collections/6227f475d29dba2ec653) and follow instructions in description of calls to get your authentication credentials.
 
 * AUTHENTICATE
    * Get your [Shopify API credentials.](https://help.shopify.com/api/guides/api-credentials)  Set up a Shopify PARTNER account, log into the partner portal and create an integration application.
Ensure ShopName, Scopes, APIKey and CallbackURL are filled in your ENV file in Postman.
    * Run the first request.
    * Press Generate Code and combine pieces so it looks similar to this:
`https://{{storeName}}.myshopify.com/admin/oauth/authorize?client_id={{clientId}}&scope=write_orders,read_customers&redirect_uri=http://localhost/`
    * Copy + paste that into your browser URL. You may have to "approve" the connection. Then you will be directed to a dead page. Copy the "Code" from the URL and place it in the ENV.
 *  Exchange CODE for your ACCESS TOKEN
    * Ensure the following fields are filled in you ENV:
        * APIKey
        * Secret
        * ShopName
        * Code

### 2. Set up webhooks

Shopify uses [webhooks](https://help.shopify.com/api/tutorials/webhooks) to push requests to subscribers when events happen in the Shopify system. Detailed information about using the webhook API calls can be viewed [at Shopify's API reference](https://help.shopify.com/api/reference/webhook).
 
1. [Launch ngrok](https://ngrok.com/docs) by using a terminal and navigating to the folder containing `ngrok.exe` and entering the following command: `ngrok http 3000`. This will create a tunnel that will redirect traffic from the ngork forwarding URL to loaclhost on port 3000. You can view traffic at the URL by going to `http://localhost:4040/`

2. Use [this Postman collection](https://www.getpostman.com/collections/6227f475d29dba2ec653) or the (webhook tool) to then subscribe to topics in Shopify using the URL provided by ngrok. The endpoint to use for the webhooks will the the ngrok supplied URL followed by `/webhook` (ex. `http://5541fdac.ngrok.io/webhook`). Topics supported are:
 * `customer/update`
 * `customer/delete` 
 * `product/create`
 * `product/update`
 * `product/delete`

**NOTE**: The `customer/create` topic should not be used because creating a customer in Shopify appears to trigger both the customer/create and customer/update topics and the action only needs to be processed once.

## Mapping

Before the App can be run, you must first create a series of Mapping values in [dev-settings.json](path to dev-settings). Configuration is done in JSON format with key value pairs.

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
