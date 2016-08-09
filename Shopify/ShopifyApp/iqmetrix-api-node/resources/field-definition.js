'use strict';
var app = require('../../app');
const assign = require('lodash/assign');
const nconf = require('nconf');
const base = require('../mixins/base');

/**
 * Creates a FieldDefinition instance.
 *
 * @param {iQmetrix} iqmetrix 	Reference to iQmetrix instance
 * @constructor
 * @public
 */
function FieldDefinition(iqmetrix) {
	this.iqmetrix = iqmetrix;
	this.service = 'productlibrary';
	this.host = `${this.service}${this.iqmetrix.baseUrl.environment}.iqmetrix.net`;
}
assign(FieldDefinition.prototype, base);

/**
 * Returns a list of the Field Definitions that exist.
 *
 * @param {Object} query 	Query for resulting data (optional)
 * @return {Promise} 		Promise that resolves with the result
 * @public
 */
FieldDefinition.prototype.retrieveAll = function (query) {
	var path = (`/v1/FieldDefinitions`);
	return this.retrieve(path, query);
};

module.exports = FieldDefinition;
