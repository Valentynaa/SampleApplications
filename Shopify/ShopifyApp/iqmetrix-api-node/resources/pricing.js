'use strict';
var app = require('../../app');
const assign = require('lodash/assign');
const nconf = require('nconf');
const base = require('../mixins/base');

/**
 * Creates a Pricing instance.
 *
 * @param {iQmetrix} iqmetrix 	Reference to iQmetrix instance
 * @constructor
 * @public
 */
function Pricing(iqmetrix) {
	this.iqmetrix = iqmetrix;
	this.service = 'pricing';
	this.host = `${this.service}${this.iqmetrix.baseUrl.environment}.iqmetrix.net`;
}
assign(Pricing.prototype, base);

/**
 * Creates an Pricing record.
 *
 * @param {Object} pricing 	Pricing to create
 * @return {Promise} 		Promise that resolves with the result
 * @public
 */
Pricing.prototype.createPricing = function (pricing) {
	var companyId = app.nconf.get('iq_settings:companyId');
	var locationId = app.nconf.get('iq_settings:locationId');
	var path = (`/v1/companies(${companyId})/Entities(${locationId})/CatalogItems(${pricing.CatalogItemId})/Pricing`);
	return this.create(path, pricing);
};

module.exports = Pricing;
