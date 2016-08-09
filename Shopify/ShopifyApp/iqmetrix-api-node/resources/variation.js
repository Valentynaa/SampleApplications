'use strict';
var app = require('../../app');
const assign = require('lodash/assign');
const nconf = require('nconf');
const base = require('../mixins/base');

/**
 * Creates a Variation instance.
 *
 * @param {iQmetrix} iqmetrix 	Reference to iQmetrix instance
 * @constructor
 * @public
 */
function Variation(iqmetrix) {
	this.iqmetrix = iqmetrix;
	this.service = 'productlibrary';
	this.host = `${this.service}${this.iqmetrix.baseUrl.environment}.iqmetrix.net`;
}
assign(Variation.prototype, base);

/**
 * Creates a Variation record.
 *
 * @param {Object} variation 	Variation to create
 * @param {Number} productId 	Product ID of Master Product
 * @return {Promise} 			Promise that resolves with the result
 * @public
 */
Variation.prototype.createVariation = function (productId, variation) {
	console.log('controller');
	var path = (`/v1/ProductDocs(${productId})/variations`);
	return this.create(path, variation);
};

/**
 * Updates a Variation record.
 *
 * @param {Object} variation 	Variation to update
 * @param {Object} variationId 	ID of Variation to update
 * @param {Object} productId 	Product ID of Master Product
 * @return {Promise} 			Promise that resolves with the result
 * @public
 */
Variation.prototype.updateVariation = function (productId, variation, variationId) {
	var id = variationId || '';
	
	if(id === '')
		id = variation.Id || '';

	var path = (`/v1/ProductDocs(${productId})/variations?variationId=${id}`);
	return this.update(path, variation);
};

module.exports = Variation;
