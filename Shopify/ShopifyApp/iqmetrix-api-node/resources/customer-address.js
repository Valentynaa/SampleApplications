'use strict';
var app = require('../../app');
const assign = require('lodash/assign');
const nconf = require('nconf');
const base = require('../mixins/base');

/**
 * Creates a CustomerAddress instance.
 *
 * @param {iQmetrix} iqmetrix 	Reference to iQmetrix instance
 * @constructor
 * @public
 */
function CustomerAddress(iqmetrix) {
	this.iqmetrix = iqmetrix;
	this.service = 'crm';
	this.host = `${this.service}${this.iqmetrix.baseUrl.environment}.iqmetrix.net`;
}
assign(CustomerAddress.prototype, base);

/**
 * Creates an Address record.
 *
 * @param {Object} address 	Address to create
 * @return {Promise} 		Promise that resolves with the result
 * @public
 */
CustomerAddress.prototype.createAddress = function (address) {
	var companyId = app.nconf.get('iq_settings:companyId');
	var path = (`/v1/companies(${companyId})/Customers(${address.CustomerId})/Addresses`);
	return this.create(path, address);
};

/**
 * Updates an Address record.
 *
 * @param {Object} address 	Address to update
 * @return {Promise} 		Promise that resolves with the result
 * @public
 */
CustomerAddress.prototype.updateAddress = function (address) {
	var companyId = app.nconf.get('iq_settings:companyId');
	var path = (`/v1/companies(${companyId})/Customers(${address.CustomerId})/Addresses(${address.Id})`);
	return this.update(path, address);
};

/**
 * Deletes an Address record.
 *
 * @param {Object} address 	Address to delete
 * @return {Promise} 		Promise that resolves with the result
 * @public
 */
CustomerAddress.prototype.deleteAddress = function (address) {
	var companyId = app.nconf.get('iq_settings:companyId');
	var path = (`/v1/companies(${companyId})/Customers(${address.CustomerId})/Addresses(${address.Id})`);
	return this.delete(path);
};

/**
 * Returns a list of the Address Types that exist.
 *
 * @param {Object} query 	Query for resulting data
 * @return {Promise} 		Promise that resolves with the result
 * @public
 */
CustomerAddress.prototype.retrieveAddressTypes = function (query) {
	var companyId = app.nconf.get('iq_settings:companyId');
	var path = (`/v1/companies(${companyId})/AddressTypes`);
	return this.retrieve(path, query);
};

module.exports = CustomerAddress;
