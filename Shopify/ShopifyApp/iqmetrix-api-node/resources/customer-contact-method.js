'use strict';
var app = require('../../app');
const assign = require('lodash/assign');
const nconf = require('nconf');
const base = require('../mixins/base');

/**
 * Creates an CustomerContactMethod instance.
 *
 * @param {iQmetrix} iqmetrix 	Reference to iQmetrix instance
 * @constructor
 * @public
 */
function CustomerContactMethod(iqmetrix) {
	this.iqmetrix = iqmetrix;
	this.service = 'crm';
	this.host = `${this.service}${this.iqmetrix.baseUrl.environment}.iqmetrix.net`;
}
assign(CustomerContactMethod.prototype, base);

/**
 * Creates a CustomerContactMethod record.
 *
 * @param {Object} contactMethod 	Contact Method to create
 * @return {Promise} 				Promise that resolves with the result
 * @public
 */
CustomerContactMethod.prototype.createContactMethod = function (contactMethod) {
	var companyId = app.nconf.get('iq_settings:companyId');
	var path = (`/v1/companies(${companyId})/Customers(${contactMethod.CustomerId})/ContactMethods`);
	return this.create(path, contactMethod);
};

/**
 * Returns a list of the Contact Method Categories that exist.
 *
 * @param {Object} query 	Query for resulting data
 * @return {Promise} 		Promise that resolves with the result
 * @public
 */
CustomerContactMethod.prototype.retrieveContactMethodCategories = function (query) {
	var companyId = app.nconf.get('iq_settings:companyId');
	var path = (`/v1/companies(${companyId})/ContactMethodCategories`);
	return this.retrieve(path, query);
};

/**
 * Returns a Contact Method Type.
 *
 * @param {Number} categoryId 	Contact Method Category ID for the Method Type
 * @return {Promise} 			Promise that resolves with the result
 * @public
 */
CustomerContactMethod.prototype.retrieveContactMethodType = function (categoryId) {
	var companyId = app.nconf.get('iq_settings:companyId');
	var path = (`/v1/companies(${companyId})/ContactMethodCategories(${categoryId})/ContactMethodTypes`);
	return this.retrieve(path);
};

/**
 * Updates a CustomerContactMethod record.
 *
 * @param {Object} contactMethod 	CustomerContactMethod to update
 * @return {Promise} 				Promise that resolves with the result
 * @public
 */
CustomerContactMethod.prototype.updateContactMethod = function (contactMethod) {
	var companyId = app.nconf.get('iq_settings:companyId');
	var path = (`/v1/companies(${companyId})/Customers(${contactMethod.CustomerId})/ContactMethods(${contactMethod.Id})`);
	return this.update(path, contactMethod);
};

/**
 * Deletes a CustomerContactMethod record.
 *
 * @param {Object} contactMethod 	CustomerContactMethod to delete
 * @return {Promise} 				Promise that resolves with the result
 * @public
 */
CustomerContactMethod.prototype.deleteContactMethod = function (contactMethod) {
	var companyId = app.nconf.get('iq_settings:companyId');
	var path = (`/v1/companies(${companyId})/Customers(${contactMethod.CustomerId})/ContactMethods(${contactMethod.Id})`);
	return this.delete(path);
};

module.exports = CustomerContactMethod;
