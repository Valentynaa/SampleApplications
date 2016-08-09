'use strict';
var app = require('../../app');
const assign = require('lodash/assign');
const nconf = require('nconf');
const base = require('../mixins/base');
const filter = require('../../utilities/filter');

/**
 * Creates a Customer instance.
 *
 * @param {iQmetrix} iqmetrix 	Reference to iQmetrix instance
 * @constructor
 * @public
 */
function Customer(iqmetrix) {
	this.iqmetrix = iqmetrix;
	this.service = 'crm';
	this.host = `${this.service}${this.iqmetrix.baseUrl.environment}.iqmetrix.net`;
}
assign(Customer.prototype, base);

/**
 * Creates a Customer record.
 *
 * @param {Object} customer 	Customer to create
 * @return {Promise} 			Promise that resolves with the result
 * @public
 */
Customer.prototype.createCustomer = function (customer) {
	var companyId = app.nconf.get('iq_settings:companyId');
	var path = (`/v1/companies(${companyId})/Customers`);
	return this.create(path, customer);
};

/**
 * Creates a Customer record.
 *
 * @param {Object} customerFull Customer to create
 * @return {Promise} 			Promise that resolves with the result
 * @public
 */
Customer.prototype.createCustomerFull = function (customerFull) {
	var companyId = app.nconf.get('iq_settings:companyId');
	var path = (`/v1/companies(${companyId})/CustomerFull`);
	return this.create(path, customerFull);
};

/**
 * Returns a list of Customer records matching the given query.
 *
 * @param {Object} query 	Query for resulting data
 * @return {Promise} 		Promise that resolves with the result
 * @public
 */
Customer.prototype.retrieveCustomerSearch = function (query) {
	var companyId = app.nconf.get('iq_settings:companyId');
	var path = `/v1/Companies(${companyId})/CustomerSearch?$filter=`;
	path += encodeURIComponent(filter.stringify(query));
	return this.retrieve(path);
}

/**
 * Returns a list of Customer Extension Types that exist.
 *
 * @param {Object} query 	Query for resulting data (optional)
 * @return {Promise} 		Promise that resolves with the result
 * @public
 */
Customer.prototype.retrieveCustomerExtensionTypes = function(query) {
	var companyId = app.nconf.get('iq_settings:companyId');
	var path = `/v1/Companies(${companyId})/CustomerExtensionTypes`;
	return this.retrieve(path, query);
}

/**
 * Returns a list of the Customer Types that exist.
 *
 * @param {Object} query 	Query for resulting data (optional)
 * @return {Promise} 		Promise that resolves with the result
 * @public
 */
Customer.prototype.retrieveCustomerTypes = function(query) {
	var companyId = app.nconf.get('iq_settings:companyId');
	var path = `/v1/Companies(${companyId})/CustomerTypes`;
	return this.retrieve(path, query);
}

/**
 * Returns a CustomerFull record.
 *
 * @param {String} customerId 	ID of customer to get CustomerFull record for
 * @return {Promise} 			Promise that resolves with the result
 * @public
 */
Customer.prototype.retrieveCustomerFull = function(customerId) {
	var companyId = app.nconf.get('iq_settings:companyId');
	var path = `/v1/Companies(${companyId})/CustomerFull(${customerId})`;
	return this.retrieve(path);
}

/**
 * Updates a Customer record.
 *
 * @param {Object} customer 	Customer to update
 * @return {Promise} 			Promise that resolves with the result
 * @public
 */
Customer.prototype.updateCustomer = function (customer) {
	var companyId = app.nconf.get('iq_settings:companyId');
	var path = (`/v1/companies(${companyId})/Customers(${customer.Id})`);
	return this.update(path, customer);
};

/**
 * Deletes a Customer record.
 *
 * @param {Object} customer 	Customer to delete
 * @return {Promise} 			Promise that resolves with the result
 * @public
 */
Customer.prototype.deleteCustomer = function (customer) {
	var companyId = app.nconf.get('iq_settings:companyId');
	var path = (`/v1/companies(${companyId})/Customers(${customer.Id})`);
	return this.delete(path);
};


module.exports = Customer;
