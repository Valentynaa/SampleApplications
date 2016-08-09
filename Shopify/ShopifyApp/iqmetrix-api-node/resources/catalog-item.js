'use strict';
var app = require('../../app');
const assign = require('lodash/assign');
const nconf = require('nconf');
const base = require('../mixins/base');

/**
 * Creates a CatalogItem instance.
 *
 * @param {iQmetrix} iqmetrix 	Reference to iQmetrix instance
 * @constructor
 * @public
 */
function CatalogItem(iqmetrix) {
	this.iqmetrix = iqmetrix;
	this.service = 'catalogs';
	this.host = `${this.service}${this.iqmetrix.baseUrl.environment}.iqmetrix.net`;
}
assign(CatalogItem.prototype, base);

/**
 * Creates a Catalog Item.
 *
 * @param {Object} catalogItem 	Catalog Item to create
 * @return {Promise} 			Promise that resolves with the result
 * @public
 */
CatalogItem.prototype.createCatalogItem = function (catalogItem) {
	var companyId = app.nconf.get('iq_settings:companyId');
	var path = (`/v1/companies(${companyId})/catalog/items`);
	return this.create(path, catalogItem);
};


CatalogItem.prototype.retrieveProductDetails = function (catalogItemId){
	var companyId = app.nconf.get('iq_settings:companyId');
	var path = (`/v1/companies(${companyId})/catalog/items(${catalogItemId})/productDetails`);
	return this.retrieve(path, catalogItemId);
}

/**
 * Returns all CatalogItems that match the criteria provided. The only valid fields for this 
 * form of search are "Slug", "Source", and "RmsId" (case insensitive)
 *
 * @param {String} field 			Field to search for value in (optional)
 * @param {String|Number} value 	Value for field (optional)
 * @return {Promise} 				Promise that resolves with the result
 * @public
 */
CatalogItem.prototype.retrieveCatalogItems = function (field, value){
	let query = '';

	if(field.match(/Slug/i) || field.match(/Source/i) || field.match(/RmsId/i)){
		query = `(${field}=${value})`;
	}

	var companyId = app.nconf.get('iq_settings:companyId');
	var path = `/v1/companies(${companyId})/catalog/items` + query;

	console.log('path', path);

	return this.retrieve(path);
}

/**
 * Deletes a Catalog Item record.
 *
 * @param {Object} catalogItemId 	ID of catalog item to delete
 * @return {Promise} 				Promise that resolves with the result
 * @public
 */
CatalogItem.prototype.deleteCatalogItem = function (catalogItemId) {
	var companyId = app.nconf.get('iq_settings:companyId');
	var path = (`/v1/companies(${companyId})/catalog/items(${catalogItemId})`);
	return this.delete(path);
};

module.exports = CatalogItem;
