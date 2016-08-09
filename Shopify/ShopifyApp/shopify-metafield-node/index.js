'use strict';

const assign = require('lodash/assign');
const base = require('./base');

/**
 * Creates a Metafield instance.
 *
 * @param {Shopify} shopify Reference to the Shopify instance
 * @constructor
 * @public
 */
function Metafield(shopify) {
	this.shopify = shopify;
}
assign(Metafield.prototype, base);

/**
 * Returns a list of the metafields associated with the supplied resource and ID
 *
 * @param {String} resource Resource to retrieve
 * @param {Number} id 		ID for resource record
 * @return {Promise} Promise that resolves with the result
 * @public
 */
Metafield.prototype.retrieveMetaFields = function retrieveMetaFields(resource, id, query) {
	const url = this.buildUrl(resource, id, query);
	return this.shopify.request(url, 'GET', 'metafields');
};

module.exports = Metafield;
