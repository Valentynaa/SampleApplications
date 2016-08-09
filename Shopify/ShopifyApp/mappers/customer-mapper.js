'use strict';

const linq = require('jslinq');
const shopifyCustomer = require('shopify-api-node/resources/customer');
const app = require('../app');
const regexValues = require('../utilities/regex');
const iqCustomer = require('../iqmetrix-api-node/resources/customer');
const iqCustomerAddress = require('../iqmetrix-api-node/resources/customer-address');
const iqCustomerContactMethod = require('../iqmetrix-api-node/resources/customer-contact-method');
const shopifyMetafield = require('../shopify-metafield-node');

//Controllers for making API calls
var customerController;
var addressController;
var contactMethodController;

var shopifyMetafieldController;
var shopifyCustomerController;

//Private variables
var addressTypeId, customerTypeId, mappingFieldId;
var loaded = false;
let customerNameKey = 'customers';

/**
 * Creates a Customer Mapper instance.
 *
 * @param {iqmetrix} iqmetrix Reference to the iQmetrix instance
 * @param {shopify} shopify Reference to the Shopify instance
 * @constructor
 * @public
 */
function CustomerMapper(iqmetrix, shopify){
	this.iqmetrix = iqmetrix;
	this.shopify = shopify;

	customerController = new iqCustomer(iqmetrix);
	addressController = new iqCustomerAddress(iqmetrix);
	contactMethodController = new iqCustomerContactMethod(iqmetrix);

	shopifyMetafieldController = new shopifyMetafield(shopify);
	shopifyCustomerController = new shopifyCustomer(shopify);
}

/**
 * Loads all mapping variables and sets the loaded flag to true. Mapping 
 * variables include: mappingFieldId, customerTypeId, and addressTypeId.
 * 
 * @return {Promise}
 */
function loadMappingVariables(){
	return new Promise(resolve => {
		getMappingFieldId().then(function(result){
			mappingFieldId = result;
			return getCustomerTypeId()
		}).then(function(result){
			customerTypeId = result;
			return getAddressTypeId()
		}).then(function(result){
			addressTypeId = result;
			loaded = true;
			resolve();
		});
	});
}

/**
 * Runs necessary creations or updates for iQmetrix based on customers changed in Shopify from the 
 * provided start time.
 *
 * @param {Date} time 	Time to sync customer updates from
 * @public
 */
CustomerMapper.prototype.syncCustomers = function (time){
	console.log('Doing customer sync from: ' + time);

	//Set the required field IDs
	loadMappingVariables().then(() => {
		getCustomersUpdated(time).then(shopifyCustomers => {
			shopifyCustomers.map(customer => {
				upsertCustomerLoaded(customer);
			});
			console.log('Customer sync complete');
		});
	});
};

/**
 * Handler for the customer/update event. Either updates or creates a customer 
 * in iQmetrix.
 *
 * @param {Object} shopifyCustomer 	Customer that was updated
 * @public
 */
CustomerMapper.prototype.updateCustomer = function (shopifyCustomer){
	if (loaded){
		upsertCustomerLoaded(shopifyCustomer);
	}else{
		loadMappingVariables().then(() => {
			upsertCustomerLoaded(shopifyCustomer);
		});
	}
}

/**
 * Handler for the customer/create event. Either updates or creates a customer 
 * in iQmetrix.
 *
 * @param {Object} shopifyCustomer 	Customer that was created
 * @public
 */
CustomerMapper.prototype.createCustomer = function (shopifyCustomer){
	if (loaded){
		upsertCustomerLoaded(shopifyCustomer);
	}else{
		loadMappingVariables().then(() => {
			upsertCustomerLoaded(shopifyCustomer);
		});
	}
}

/**
 * Handler for the customer/delete event. Disables the customer record in iQmetrix
 *
 * @param {Object} shopifyCustomer 	Customer that was deleted
 * @public
 */
CustomerMapper.prototype.deleteCustomer = function (shopifyCustomer){
	customerController.retrieveCustomerSearch({Criteria: shopifyCustomer.id.toString()}).then(customersMapped => {
		customersMapped.map(customer => {
			console.log('Deleting customer ' + customer.Id)
			customerController.deleteCustomer(customer);
		});
	});
}

/**
 * Gets the Mapping Field Id used for iQmetrix
 *
 * @return {Promise} 
 */
function getMappingFieldId(){
	return customerController.retrieveCustomerExtensionTypes().then(function(result){
		return linq(result).firstOrDefault(function(x) {return x.Name === app.nconf.get('iq_settings:customerExtensionType')});
	}).then(function(result){
		return result.Id;
	});
}

/**
 * Gets the Customer Type Id for the 'Person' type
 *
 * @return {Promise}
 */
function getCustomerTypeId(){
	return customerController.retrieveCustomerTypes().then(function(result){
		return linq(result).firstOrDefault(function(x) {return x.Name === app.nconf.get('iq_settings:customerType')});
	}).then(function(result){
		return result.Id;
	});
}

/**
 * Gets the Address Type Id for the 'Shipping' type
 *
 * @return {Promise}
 */
function getAddressTypeId(){
	return addressController.retrieveAddressTypes().then(function(result){
		return linq(result).firstOrDefault(function(x) {return x.Name === app.nconf.get('iq_settings:addressType')});
	}).then(function(result){
		return result.Id;
	});
}

/**
 * Gets the shopify customers that have been updated since the time provided
 *
 * @param {Date} time 	Time to get updates from
 * @return {Promise} Shopify Customers that have changes
 */
function getCustomersUpdated(time){
	return shopifyCustomerController.search().then(function(result){
	 	return linq(result).where(function(x) {return new Date(x.updated_at) > time}).items;
	});
}

/**
 * Either creates or updates a customer for iQmetrix based on whether a mapping exists.
 * Mapping to Shopify customer is in CustomerExtensions on the iQmetrix customer
 * Mapping to iQmetrix customer is in metafields on the Shopify customer
 *
 * @param {Object} shopifyCustomer 	Customer to upsert
 */
function upsertCustomerLoaded(shopifyCustomer){
	//Determine if new or existing customer
	//Do insert or update accordingly
	hasMapping(shopifyCustomer, customerNameKey).then(function(result){
		if(result.mapped){
			console.log('Doing customer update for ' + result.customer.id);
			updateCustomerLoaded(result.customer);
		}
		else{
			console.log('Doing customer creation for ' + result.customer.id);
			createCustomerLoaded(result.customer);
		}
	});
}

/**
 * Creates and maps a customer and their addresses to iQmetrix.
 * Mapping to Shopify customer is in CustomerExtensions on the iQmetrix customer
 * Mapping to iQmetrix customer is in metafields on the Shopify customer
 *
 * @param {Object} shopifyCustomer 	Customer to map
 */
function createCustomerLoaded(shopifyCustomer){
	//Build the generic customer object for iQmetrix including mapping to Shopify customer ID
	let newCustomer = {
		PrimaryName: shopifyCustomer.first_name || '',
		MiddleName: '',
		FamilyName: shopifyCustomer.last_name || '',
		Addresses: [],
		ContactMethods: [],
		CustomerExtensions: [
			{
				ExtensionTypeId: mappingFieldId,
				Value: shopifyCustomer.id
			}
		],
		CustomerTypeId: customerTypeId,
		Disabled: false
	}

	//Add Addresses if they exist
	if('addresses' in shopifyCustomer){
		shopifyCustomer.addresses.map(address => {
			newCustomer.Addresses.push(iQAddressFromShopifyAddress(address, shopifyCustomer.email));
		});
	}

	//Create and map customer
	customerController.createCustomerFull(newCustomer).then(function(customer){
		console.log('Mapping ' + shopifyCustomer.id + ' to ' + customer.Id);
		shopifyCustomerController.update(shopifyCustomer.id, {
			id: shopifyCustomer.id,
			metafields: [
				{
					key: app.nconf.get('iq_settings:mappingFieldName'),
					value: customer.Id,
					value_type: 'string',
					namespace: 'global'
				}
			]
		})
	});
}

/**
 * Updates a shopify customer and their addresses for iQmetrix.
 *
 * NOTE: 
 *	shopifyCustomer must have 'metafields' property added to it prior to function.
 * 	Mapping variables must be initialized.
 *
 * @param {Object} shopifyCustomer 	Customer to update
 */
function updateCustomerLoaded(shopifyCustomer){
	var iqmetrixCustomerId = linq(shopifyCustomer.metafields)
		.firstOrDefault(function(x) {return x.key === app.nconf.get('iq_settings:mappingFieldName')}).value;

	var shopifyAddresses = ('addresses' in shopifyCustomer) ? shopifyCustomer.addresses : [];
	var iqmetrixAddresses = [];
	var toCreate = [], toDelete = [];

	customerController.retrieveCustomerFull(iqmetrixCustomerId).then(function (customerFull){
		//Update Addresses
		iqmetrixAddresses = customerFull.Addresses;

		//Go through shopify addresses to find new ones
		for (var i = 0; i < shopifyAddresses.length; ++i){
			var postCode = getValidPostCode(shopifyAddresses[i]);
			if(!linq(iqmetrixAddresses)
				.any(function(x) {
					return x.StreetAddress1 === shopifyAddresses[i].address1 && 
					x.StreetAddress2 === shopifyAddresses[i].address2 &&
					x.StateCode === shopifyAddresses[i].province_code && 
					x.State === shopifyAddresses[i].province && 
					x.PostalCode === postCode && 
					x.CountryCode === shopifyAddresses[i].country_code && 
					x.Country === shopifyAddresses[i].country
				})
			){
				toCreate.push(shopifyAddresses[i]);
			}
		}

		//Go through iqmetrix addresses to find ones to delete
		for (var i = 0; i < iqmetrixAddresses.length; ++i){
			if(!linq(shopifyAddresses)
				.any(function(x) {
					return iqmetrixAddresses[i].StreetAddress1 === x.address1 && 
					iqmetrixAddresses[i].StreetAddress2 === x.address2 &&
					iqmetrixAddresses[i].StateCode === x.province_code && 
					iqmetrixAddresses[i].State === x.province && 
					iqmetrixAddresses[i].PostalCode === postCode && 
					iqmetrixAddresses[i].CountryCode === x.country_code && 
					iqmetrixAddresses[i].Country === x.country
				})
			){
				toDelete.push(iqmetrixAddresses[i]);
			}
		}

		//Do all creations and deletions for addresses necessary
		for (var i = 0; i < toCreate.length; ++i){
			addressController.createAddress(iQAddressFromShopifyAddress(toCreate[i], shopifyCustomer.email, 
				customerFull.Id));
		}

		for (var i = 0; i < toDelete.length; ++i){
			addressController.deleteAddress(toDelete[i]);
		}

		//Update customer itself
		if(customerFull.PrimaryName !== shopifyCustomer.first_name ||
			customerFull.FamilyName !== shopifyCustomer.last_name
		){
			customerController.updateCustomer({
				Id: customerFull.Id,
				CustomerTypeId: customerTypeId,
				PrimaryName: shopifyCustomer.first_name,
				FamilyName: shopifyCustomer.last_name
			});
		}
		console.log('All Customer Update calls made for ' + shopifyCustomer.id);
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
 * Validates a postcode and countrycode pair to ensure a valid postcode is returned.
 * If the postcode is invalid then '' is returned.
 *
 * @param {Object} shopifyAddress 	Address to check post code on
 * @return {String} Valid post code
 */
function getValidPostCode(shopifyAddress){
	var postCode = shopifyAddress.zip;
	var countryCode = shopifyAddress.country_code;
	var result = postCode;
	if(countryCode === 'CA' || countryCode === 'US' || countryCode === 'AU'){
		var regex = regexValues.postCode[countryCode];
		if (!regex.test(postCode)){
			result = '';
		}
		regex.lastIndex = 0;
		result.replace(' ', '').replace('-', '');
	}
	return result;
}

/**
 * Creates an equivalent customer address for iQmetrix from an address on a
 * shopify customer.
 * 
 * NOTE: 
 * 	If performing an update with the address returned, the customerId parameter
 *	must be included, otherwise it is unnecessary.
 *
 * @param {Object} shopifyAddress 			Shopify address to get data from
 * @param {String} email (optional)			Email for address
 * @param {String} customerId (see NOTE)	Customer ID for updating an existing address
 * @return {Object} Customer Address for iQmetrix
 */
function iQAddressFromShopifyAddress(shopifyAddress, email, customerId){
	var postCode = getValidPostCode(shopifyAddress);
	var address = {
		AddressTypeId: addressTypeId,
		CountryCode: shopifyAddress.country_code,
		Country: shopifyAddress.country,
		Default: shopifyAddress.default,
		DoNotContact: true,
		Email: email || '',
		Phone: shopifyAddress.phone || '',
		PostalCode: postCode || '',
		StateCode: shopifyAddress.province_code || '',
		StreetAddress1: shopifyAddress.address1 || '',
		StreetAddress2: shopifyAddress.address2 || ''
	};
	if(customerId){
		address.CustomerId = customerId;
	}
	return address;
}

module.exports = CustomerMapper;
