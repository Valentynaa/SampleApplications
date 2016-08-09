'use strict';

const linq = require('jslinq');
const q = require('q');
const colors = require('colors/safe');
const util = require('util');
const md5File = require('md5-file/promise');
const shopifyProduct = require('shopify-api-node/resources/product');
const shopifyVariant = require('shopify-api-node/resources/product-variant');
const app = require('../app');
const regexValues = require('../utilities/regex');
const iqAsset = require('../iqmetrix-api-node/resources/asset');
const iqFieldDefinition = require('../iqmetrix-api-node/resources/field-definition');
const iqClassificationTree = require('../iqmetrix-api-node/resources/classification-tree');
const iqMasterProduct = require('../iqmetrix-api-node/resources/master-product');
const iqCatalogItem = require('../iqmetrix-api-node/resources/catalog-item');
const iqVariation = require('../iqmetrix-api-node/resources/variation');
const iqAvailability = require('../iqmetrix-api-node/resources/availability');
const iqPricing = require('../iqmetrix-api-node/resources/pricing');
const shopifyMetafield = require('../shopify-metafield-node');
const file = require('../utilities/file');

//Controllers for making API calls
//Shopify Controllers
let shopifyProductController;
let shopifyVariantController;
let shopifyMetafieldController;

//iQmetrix Controllers
let iqFieldDefinitionController;
let iqClassificationTreeController;
let iqAssetController;
let iqMPController;
let iqCatalogItemController;
let iqVariationController;
let iqAvailabilityController;
let iqPricingController;

//Other private variables
let loaded = false;

//Names used by Shopfy to identify resources
let productNameKey = 'products';
let variantNameKey = 'variants';

/**
 * Creates a Product Mapper instance. Initializes Controllers
 *
 * @param {iqmetrix} iqmetrix Reference to the iQmetrix instance
 * @param {shopify} shopify Reference to the Shopify instance
 * @constructor
 * @public
 */
function ProductMapper(iqmetrix, shopify){
	this.iqmetrix = iqmetrix;
	this.shopify = shopify;

	iqFieldDefinitionController = new iqFieldDefinition(iqmetrix);
	iqClassificationTreeController = new iqClassificationTree(iqmetrix);
	iqAssetController = new iqAsset(iqmetrix);
	iqMPController = new iqMasterProduct(iqmetrix);
	iqCatalogItemController = new iqCatalogItem(iqmetrix);
	iqVariationController = new iqVariation(iqmetrix);
	iqAvailabilityController = new iqAvailability(iqmetrix);
	iqPricingController = new iqPricing(iqmetrix);

	shopifyProductController = new shopifyProduct(shopify);
	shopifyVariantController = new shopifyVariant(shopify);
	shopifyMetafieldController = new shopifyMetafield(shopify);
}

/**
 * Runs necessary creations or updates for iQmetrix based on products changed in 
 * Shopify from the provided start time.
 *
 * @param {Date} time 	Time to sync product updates from
 * @public
 */
ProductMapper.prototype.syncProducts = function (time){
	console.log('Doing Product sync from: ' + time);

	//Set the required field IDs
	loadMappingVariables().then(() => {
		//Get products updated
		getProductsUpdated(time).then(shopifyProducts => {
			shopifyProducts.map(product => {
				upsertProductLoaded(product);
			});
			console.log('Product sync complete');
		});
	});
};

/**
 * Handler for the product/update event. Either updates or creates a product 
 * in iQmetrix.
 *
 * @param {Object} shopifyProduct 	Product that was updated
 * @public
 */
ProductMapper.prototype.updateProduct = function (shopifyProduct){
	if (loaded){
		upsertProductLoaded(shopifyProduct);
	}else{
		loadMappingVariables().then(() => {
			upsertProductLoaded(shopifyProduct);
		});
	}
}

/**
 * Handler for the product/create event. Either updates or creates a product 
 * in iQmetrix.
 *
 * @param {Object} shopifyProduct 	Product that was created
 * @public
 */
ProductMapper.prototype.createProduct = function (shopifyProduct){
	if (loaded){
		upsertProductLoaded(shopifyProduct);
	}else{
		loadMappingVariables().then(() => {
			upsertProductLoaded(shopifyProduct);
		});
	}
}

/**
 * Handler for the product/delete event. Either updates or creates a product 
 * in iQmetrix.
 *
 * @param {Object} shopifyProduct 	Product that was deleted
 * @public
 */
ProductMapper.prototype.deleteProduct = function (shopifyProduct){
	if (loaded){
		deleteProductLoaded(shopifyProduct);
	}else{
		loadMappingVariables().then(() => {
			deleteProductLoaded(shopifyProduct);
		});
	}
}

/**
 * Loads all mapping variables and sets the loaded flag to true. Mapping 
 * variables include: 
 * 
 * NOTE:
 *	Currently no mapping variables are required to be initialzed. This is a place holder function 
 *  in the event that a mapping variable (like iQmetrix Classification Tree) is required.
 * 
 * @return {Promise}
 */
function loadMappingVariables(){
	return new Promise(resolve => {
		loaded = true;
		resolve();	
	});	
}

/**
 * Gets the shopify products that have been updated since the time provided
 *
 * @param {Date} time 	Time to get updates from
 * @return {Promise} 	Shopify Products that have changes
 */
function getProductsUpdated(time){
	return shopifyProductController.list().then(result => {
		return linq(result).where(x => {return new Date(x.updated_at) > time}).items;
	});
}

/**
 * Checks if a shopify resource has a mapping.
 * Adds metafields for the resource to the resource object.
 *
 * @param {Object} resource 		Shopify Resource that mapping is checked for
 * @param {String} resourceName 	Name used by Shopify to identify the resource (see shopify-api-node/resources)
 * @return {Promise} 				Object with bool 'mapped' property and 'value' that was checked 
 */
function hasMapping(resource, resourceName){
	return shopifyMetafieldController.retrieveMetaFields(resourceName, resource.id).then(metafields => {
		resource.metafields = metafields;
		return {
			mapped: linq(metafields).any(x => {return x.key === app.nconf.get('iq_settings:mappingFieldName')}),
			value: resource
		};
	});
}

/**
 * Deletes the catalog item for the shopify product provided
 *
 * @param {Object} shopifyProduct 	Product deleted
 */
function deleteProductLoaded(shopifyProduct){
	iqCatalogItemController.retrieveCatalogItems('RmsId', shopifyProduct.id).then(result => {
		result.Items.map(catalogItem => {
			iqCatalogItemController.deleteCatalogItem(catalogItem.CatalogItemId);
		});
	});
}


/**
 * Either creates or updates a product for iQmetrix based on whether a mapping exists.
 * Mapping to Shopify product is in RmsId on the iQmetrix CatalogItem
 * Mapping to iQmetrix product is in metafields on the Shopify product
 *
 * @param {Object} shopifyProduct 	Product to upsert
 */
function upsertProductLoaded(shopifyProduct){
	//Determine if product is mapped or not
	hasMapping(shopifyProduct, productNameKey).then(result => {
		if(result.mapped){
			//Do updates
			console.log('Doing product update for ' + result.value.id);
			updateProductLoaded(result.value);
		}
		else{
			//Do creations
			console.log('Doing product creation for ' + result.value.id);
			createProductLoaded(result.value);
		}
	});
}

/**
 * Updates a shopify product for iQmetrix.
 *
 * NOTE: 
 *	shopifyProduct must have 'metafields' property added to it prior to function.
 * 	Mapping variables must be initialized.
 *
 * @param {Object} shopifyProduct 	Product to update
 */
function updateProductLoaded(shopifyProduct){
	let catalogItemId = linq(shopifyProduct.metafields).firstOrDefault(x => {return x.key === app.nconf.get('iq_settings:mappingFieldName')}).value;
	
	iqCatalogItemController.retrieveProductDetails(catalogItemId).then(productDetails => {
		//Get the assets on the iQmetrix product (with checksum) 
		let assets = productDetails.Assets.map(asset => {
			return new Promise(resolve => {
				iqAssetController.retrieveAsset(asset.Id).then(result => {
					resolve(result);
				});
			});
		});

		Promise.all(assets).then(assets => {
			//Download and generate hash values for all shopify images
			let promises = shopifyProduct.images.map(image => {
				return new Promise(resolve => {
					file.download(image.src).then(fileName => {
						md5File(app.rootDirectory + `\\${fileName}`).then(hash => {
							resolve({
								name: fileName,
								src: image.src,
								checksum: hash
							});
						})
					})
				})
			});

			let existingAssets = [];
			let newImages = [];
			
			q.allSettled(promises).then(results => {
				results.map(image => {
					//Success
					if(image.state === 'fulfilled'){
						//Determine if assets need to be created in iQmetrix
						let asset = linq(assets).firstOrDefault(x => {return x.name === image.value.name && x.md5Checksum == image.value.checksum});
						if (asset){
							existingAssets.push(asset);
						}else{						
							newImages.push(image.value);
						}
					}
					//Failure
					if(image.state === 'rejected'){
						console.log(colors.red(asset.reason));
					}
				});

				//Create new assets
				let newAssetPromises = newImages.map(image => {
					return new Promise(resolve => {	
						iqAssetController.createAsset(image.name).then(asset => {
							resolve(asset);
						})
					})
				});

				let newAssets = [];
				q.allSettled(newAssetPromises).then(results => {
					results.map(asset => {
						//Success
						if(asset.state === 'fulfilled'){
							newAssets.push({
								Id: asset.value.id,
								Name: asset.value.name,
								MimeType: asset.value.mimeType
							});
						}
						//Failure
						if(asset.state === 'rejected'){
							console.log(colors.red(asset.reason));
						}
					});
					
					//Build Master Product
					let masterProduct = {
						FieldValues: [
							{
								FieldDefinitionId: app.nconf.get('fieldMapping:title'),
								LanguageInvariantValue: shopifyProduct.title
							},
							{
								FieldDefinitionId: app.nconf.get('fieldMapping:body_html'),
								LanguageInvariantValue: shopifyProduct.body_html
							}
						],
						Assets: newAssets
					}
					return iqMPController.updateMasterProduct(masterProduct, productDetails.MasterProductId);
				}).then(masterProduct => {
					otherProductAdjustments(shopifyProduct, productDetails.MasterProductId).then(() => {
						console.log('Product update for', shopifyProduct.id, 'complete');
					});
				});
			});
		});
	});
}

/**
 * Creates and maps a product and their addresses to iQmetrix.
 * Mapping to Shopify product is in RmsId on the iQmetrix CatalogItem
 * Mapping to iQmetrix product is in metafields on the Shopify product
 *
 * @param {Object} shopifyProduct 	Product to map
 */
function createProductLoaded(shopifyProduct){
	let productDocId;

	//Map Category
	let category = shopifyProduct[app.nconf.get('shopify_settings:productCategory')].trim().replace(/ /g,"_");
	let categoryId = app.nconf.get(`categoryMapping:${category}`);
	
	//Map Manufacturer (optional mapping based on settings)
	let manufacturerId;
	let manufacturerField = app.nconf.get('shopify_settings:productManufacturer');
	if (manufacturerField && shopifyProduct[manufacturerField]){
		let manufacturer = shopifyProduct[manufacturerField].trim().replace(/ /g,"_");
		manufacturerId = app.nconf.get(`manufacturerMapping:${manufacturer}`);
	}

	//Create Assets
	let promises = shopifyProduct.images.map(image => {
		return new Promise(resolve => {
			file.download(image.src).then(result => {
				iqAssetController.createAsset(result).then(asset => {
					resolve(asset);
				});
			});
		});
	});

	let assets = [];
	q.allSettled(promises).then(results => {
		results.map(asset => {
			//Success
			if(asset.state === 'fulfilled'){
				assets.push({
					Id: asset.value.id,
					Name: asset.value.name,
					MimeType: asset.value.mimeType
				});
			}
			//Failure
			if(asset.state === 'rejected'){
				console.log(colors.red(asset.reason));
			}
		});

		//Create the Master Product
		let masterProduct = {
			RootRevision: {
				FieldValues: [
					{
						FieldDefinitionId: app.nconf.get('fieldMapping:title'),
						LanguageInvariantValue: shopifyProduct.title
					},
					{
						FieldDefinitionId: app.nconf.get('fieldMapping:body_html'),
						LanguageInvariantValue: shopifyProduct.body_html
					}
				],
				Assets: assets
			},
			Classification: {
				TreeId: app.nconf.get('iq_settings:classificationTreeId'),
				Id: categoryId
			},
			OwnerEntityId: app.nconf.get('iq_settings:companyId')
		};

		if(manufacturerId){
			masterProduct.Manufacturer = {
				Id: manufacturerId
			};
		}
		return iqMPController.createMasterProduct(masterProduct);
	}).then(result => {
		let slug = 'M' + result.Id;
		productDocId = result.Id;

		//Create catalog item mapped to shopify ID
		return iqCatalogItemController.createCatalogItem({
			Slug: slug,
			RmsId: shopifyProduct.id
		});
	}).then(result => {
		//Create mapping on shopify product
		return shopifyProductController.update(shopifyProduct.id, {
			id: shopifyProduct.id,
			metafields: [
				{
					key: app.nconf.get('iq_settings:mappingFieldName'),
					value: result.CatalogItemId,
					value_type: 'string',
					namespace: 'global'
				}
			]
		});
	}).then(() => {
		otherProductAdjustments(shopifyProduct, productDocId).then(() => {
			console.log('Product creation for', shopifyProduct.id, 'complete');
		});
	});
}

/**
 * Does all aspects required for product mapping outside creating the product itself.
 * This includes: creating variants, availability, and pricing for iQmetrix.
 *
 * @param {Object} shopifyProduct 	Shopify product that is being mapped
 * @param {Number} productDocId		ID for master product in iQmetrix
 * @return {Promise} 				Resolves when all actions are complete
 */
function otherProductAdjustments(shopifyProduct, productDocId){
	return new Promise(resolve => {
		upsertVariants(shopifyProduct, productDocId).then(() => {
			setAvailabilityForProduct(shopifyProduct).then(() => {
				setPricingForProduct(shopifyProduct).then(() => {
					resolve();
				});
			});
		});
	});
}

/**
 * Maps the inventory in Shopify for the product and all its variants to availability 
 * for iQmetrix 
 *
 * @param {Object} shopifyProduct 	Shopify product that is being mapped
 * @return {Promise} 				Resolves with all Availability records created
 */
function setAvailabilityForProduct(shopifyProduct){
	return new Promise(resolve => {
		if(shopifyProduct.variants.length > 1){
			//Product with variations
			let availabilityPromises = shopifyProduct.variants.map(variant => {
				return new Promise(resolve => {
					let catalogItemId = linq(variant.metafields)
						.firstOrDefault(x => {return x.key === app.nconf.get('iq_settings:mappingFieldName')}).value;

					let availability = getAvailabilityFromVariant(catalogItemId, variant);
					resolve(iqAvailabilityController.createAvailability(availability));
				});
			});

			//Update availabilities for all variations before resolving
			resolve(q.allSettled(availabilityPromises));
		}else{
			//Product with no variations
			let availability = getAvailabilityFromVariant(catalogItemId, shopifyProduct.variants[0]);
			resolve(iqAvailabilityController.createAvailability(availability));
		}		
	});
}

/**
 * Returns an Availability object that matches the inventory of the variant
 * passed in.
 *
 * @param {Object} catalogItemId 	ID for Catalog Item that availability is on
 * @param {Object} variant 			Shopify variant for availability to match
 * @return {Object} 				Availability object
 */
function getAvailabilityFromVariant(catalogItemId, variant){
	return {
		Id: catalogItemId,
		EntityId: app.nconf.get('iq_settings:locationId'),
		Quantity: variant.inventory_quantity,
		IsSerialized: false,
		IsDropShipable: false
	};
}

/**
 * Maps the price in Shopify for the product and all its variants to Pricing records 
 * for iQmetrix 
 *
 * @param {Object} shopifyProduct 	Shopify product that is being mapped
 * @return {Promise} 				Resolves with all Pricing records created
 */
function setPricingForProduct(shopifyProduct){
	return new Promise(resolve => {
		if(shopifyProduct.variants.length > 1){
			//Product with variations
			let promises = shopifyProduct.variants.map(variant => {
				let catalogItemId = linq(variant.metafields)
					.firstOrDefault(x => {return x.key === app.nconf.get('iq_settings:mappingFieldName')}).value;

				let pricing = getPricingFromVariant(catalogItemId, variant);
				return iqPricingController.createPricing(pricing);
			});
			//Update pricing for all variations before resolving
			resolve(q.allSettled(promises));
		}else{
			//Product with no variations
			let pricing = getPricingFromVariant(catalogItemId, variants[0]);
			resolve(iqPricingController.createPricing(pricing));
		}		
	});
}

/**
 * Returns a Pricing object at the location specified in 'dev-settings' that 
 * matches the price of the variant passed in.
 *
 * @param {Object} catalogItemId 	ID for Catalog Item that pricing is on
 * @param {Object} variant 			Shopify variant for pricing to match
 * @return {Object} 				Pricing object
 */
function getPricingFromVariant(catalogItemId, variant){
	return {
		CompanyId: app.nconf.get('iq_settings:companyId'),
		EntityId: app.nconf.get('iq_settings:locationId'),
		CatalogItemId: catalogItemId,
		PricingTermId: null,
		RegularPrice: variant.price,
		OverridePrice: null,
		IsDiscountable: false,
  		FloorPrice: null
	};
}

/**
 * Upserts any variants on the shopify product passed in as variations for
 * the Master Product that the productDocId applies to.
 *
 * @param {Object} shopifyProduct 	Product that variant is on
 * @param {Number} productDocId 	ID for Master Product
 * @return {Promise} 				Resolves when all variants are upserted
 */
function upsertVariants(shopifyProduct, productDocId){
	return new Promise(resolve => {
		if(shopifyProduct.variants.length > 1){
			console.log('Updating varitations for', shopifyProduct.id);
			
			let variationPromises = shopifyProduct.variants.map(variant => {
				return new Promise(resolve => {
					resolve(upsertVariation(shopifyProduct, variant, productDocId));
				})				
			});
			resolve(q.allSettled(variationPromises));
		}else{
			resolve();
		}
	});
}

/**
 * Checks if a variant has a mapping and updates or creates the respective 
 * variation for the variant in iQmetrix.
 *
 * @param {Object} shopifyProduct 	Product that variant is on
 * @param {Object} variant 			Variant for upsert
 * @param {Number} productDocId 	ID for Master Product in iQmetrix 
 * @return {Promise} 				Resolves with variation upserted
 */
function upsertVariation(shopifyProduct, variant, productDocId){
	let option1FieldId = app.nconf.get('fieldMapping:option1');
	let option2FieldId = app.nconf.get('fieldMapping:option2');
	let option3FieldId = app.nconf.get('fieldMapping:option3');
	let variation = {
		FieldValues: [
			{
				FieldDefinitionId: app.nconf.get('fieldMapping:title'),
				LanguageInvariantValue: shopifyProduct.title + variant.id
			}
		]
	};
	if(option1FieldId){
		variation.FieldValues.push({
			FieldDefinitionId: option1FieldId,
			LanguageInvariantValue: variant.option1
		});
	}
	if(option2FieldId){
		variation.FieldValues.push({
			FieldDefinitionId: option2FieldId,
			LanguageInvariantValue: variant.option2
		});
	}
	if(option3FieldId){
		variation.FieldValues.push({
			FieldDefinitionId: option3FieldId,
			LanguageInvariantValue: variant.option3
		});
	}

	return new Promise(resolve => {
		hasMapping(variant, variantNameKey).then(result => {
			if(result.mapped){
				//Do updates
				console.log('Doing variation update for ' + result.value.id);
				resolve(updateVariation(result.value, variation, productDocId));
			}
			else{
				//Do creations
				console.log('Doing variation creation for ' + result.value.id);
				resolve(createVariation(result.value, variation, productDocId));
			}
		});
	});
}

/**
 * Updates a variation in iQmetrix based on the variant used.
 *
 * @param {Object} variant 			Variant to base variation off
 * @param {Object} shopifyProduct 	Shopify Product that variant is on
 * @param {Object} productDocId 	ID for Master Product
 * @return {Promise} 				Resolves with variation updated
 */
function updateVariation(variant, variationToUpdate, productDocId){
	return new Promise(resolve => {
		let catalogItemId = linq(variant.metafields)
			.firstOrDefault(function(x) {return x.key === app.nconf.get('iq_settings:mappingFieldName')}).value;
		
		iqCatalogItemController.retrieveProductDetails(catalogItemId).then(productDetails => {
			iqVariationController.updateVariation(productDocId, variationToUpdate, productDetails.VariationId).then(variation => {
				console.log('Variation update completed for ' + variant.id);
				resolve(variation);
			});
		});	
	});
}

/**
 * Creates and maps a variation in iQmetrix based on the variant used.
 * Mapping to Shopify variant is in RmsId on the iQmetrix CatalogItem
 * Mapping to iQmetrix variation is in metafields on the Shopify variant
 *
 * @param {Object} variant 				Variant to map to
 * @param {Object} variationToCreate 	Variation object to create
 * @param {Object} productDocId 		ID for Master Product in iQmetrix
 * @return {Promise} 					Resolves with variation created
 */
function createVariation(variant, variationToCreate, productDocId){
	return new Promise(resolve => {
		iqVariationController.createVariation(productDocId, variationToCreate).then(variation => {
			let slug = `M${productDocId}-V${variation.VariationId}`;
			return iqCatalogItemController.createCatalogItem({
				Slug: slug,
				RmsId: `${variant.product_id}-${variant.id}`
			});
		}).then(catalogItem => {
			resolve(catalogItem);
			shopifyVariantController.update(variant.id, {
				id: variant.id,
				metafields: [
					{
						key: app.nconf.get('iq_settings:mappingFieldName'),
						value: catalogItem.CatalogItemId,
						value_type: 'string',
						namespace: 'global'
					}
				]
			});
		}).then(() => {
			console.log('Variation creation completed for ' + variant.id);
		});
	});
}

module.exports = ProductMapper;
