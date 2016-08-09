'use strict';
var app = require('../../app');
const assign = require('lodash/assign');
const nconf = require('nconf');
const base = require('../mixins/base');

/**
 * Creates an Availability instance.
 *
 * @param {iQmetrix} iqmetrix 	Reference to iQmetrix instance
 * @constructor
 * @public
 */
function Availability(iqmetrix) {
	this.iqmetrix = iqmetrix;
	this.service = 'availability';
	this.host = `${this.service}${this.iqmetrix.baseUrl.environment}.iqmetrix.net`;
}
assign(Availability.prototype, base);

/**
 * Creates an Availability record.
 *
 * @param {Object} availability 	Availability to create
 * @return {Promise} 				Promise that resolves with the result
 * @public
 */
Availability.prototype.createAvailability = function (availability) {
	var companyId = app.nconf.get('iq_settings:companyId');
	var path = (`/v1/companies(${companyId})/CatalogItems`);
	return this.create(path, availability);
};

module.exports = Availability;
