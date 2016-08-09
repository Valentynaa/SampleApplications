'use strict';
var app = require('../../app');
const assign = require('lodash/assign');
const nconf = require('nconf');
const base = require('../mixins/base');

/**
 * Creates a Manufacturer instance.
 *
 * @param {iQmetrix} iqmetrix 	Reference to iQmetrix instance
 * @constructor
 * @public
 */
function Manufacturer(iqmetrix) {
	this.iqmetrix = iqmetrix;
	this.service = 'entitymanager';
	this.host = `${this.service}${this.iqmetrix.baseUrl.environment}.iqmetrix.net`;
}
assign(Manufacturer.prototype, base);

/**
 * Returns a list of Manufacturers that exist.
 *
 * @param {Object} query 	Query for resulting data (optional)
 * @return {Promise} 		Promise that resolves with the result
 * @public
 */
Manufacturer.prototype.retrieveAll = function (query) {
	var path = (`/v1/Manufacturers`);
	return this.retrieve(path, query);
};

module.exports = Manufacturer;
