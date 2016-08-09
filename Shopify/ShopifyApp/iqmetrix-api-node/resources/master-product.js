'use strict';
var app = require('../../app');
const assign = require('lodash/assign');
const nconf = require('nconf');
const base = require('../mixins/base');

/**
 * Creates a MasterProduct instance.
 *
 * @param {iQmetrix} iqmetrix 	Reference to iQmetrix instance
 * @constructor
 * @public
 */
 function MasterProduct(iqmetrix) {
	this.iqmetrix = iqmetrix;
	this.service = 'productlibrary';
	this.host = `${this.service}${this.iqmetrix.baseUrl.environment}.iqmetrix.net`;
}
assign(MasterProduct.prototype, base);

/**
 * Creates a Master Product record.
 *
 * @param {Object} masterProduct 	Product to create
 * @return {Promise} 				Promise that resolves with the result
 * @public
 */
MasterProduct.prototype.createMasterProduct = function (masterProduct) {
	var path = (`/v1/ProductDocs`);
	return this.create(path, masterProduct);
};

/**
 * Updates a Master Product record.
 *
 * @param {Object} masterProduct 	Product to update
 * @param {Number} productId	 	Product ID for updated product
 * @return {Promise} 				Promise that resolves with the result
 * @public
 */
MasterProduct.prototype.updateMasterProduct = function (masterProduct, productId) {
	var id = productId || '';
	if(id === ''){
		id = masterProduct.Id || '';
	}

	var path = (`/v1/ProductDocs/${id}`);
	return this.update(path, masterProduct);
};

/**
 * Updates the current HeroShot (primary image) on a Product.
 *
 * @param {Object} asset 	Asset to set as Hero Shot
 * @param {String} slug 	Slug for Product to set Hero Shot on 
 * @return {Promise} 		Promise that resolves with the result
 * @public
 */
MasterProduct.prototype.updateHeroShot = function (asset, slug) {
	var path = (`/ProductManager/products/${slug}/heroshot`);
	return this.update(path, asset);
};

/**
 * Deletes a Master Product record.
 *
 * @param {Object} masterProduct 	Master Product to delete
 * @param {Object} productId 		Product ID for product to delete
 * @return {Promise} 				Promise that resolves with the result
 * @public
 */
MasterProduct.prototype.deleteMasterProduct = function (masterProduct, productId) {
	var id = productId || '';
	if(id === ''){
		id = masterProduct.Id || '';
	}

	var path = (`/v1/ProductDocs/${id}`);
	return this.delete(path);
};

module.exports = MasterProduct;
