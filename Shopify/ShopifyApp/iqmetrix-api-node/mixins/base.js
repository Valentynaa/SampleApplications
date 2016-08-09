'use strict';


const base = {

	/**
	 * Creates a new record.
	 *
	 * @param {String} path 		Path for API endpoint
	 * @param {Object} toCreate 	Record to create
	 * @param {String} filePath  	Path to a file that will be added to the request body (optional)
	 * @return {Promise} 			Promise	that resolves with the result
	 * @public
	 */
	create(path, toCreate, filePath) {
		return this.iqmetrix.request(this.host, path, 'POST', toCreate, filePath);
	},

	/**
	 * Deletes a record.
	 *
	 * @param {String} path 	Path for API endpoint
	 * @return {Promise} 		Promise that resolves with the result
	 * @public
	 */
	delete(path) {
		return this.iqmetrix.request(this.host, path, 'DELETE');
	},

	/**
	 * Gets a record.
	 *
	 * @param {String} path 	Path for API endpoint
	 * @param {Object} query 	Query parameters (optional)
	 * @return {Promise} 		Promise that resolves with the result
	 * @public
	 */
	retrieve(path, query) {
		return this.iqmetrix.request(this.host, path, 'GET', query);
	},

	/**
	 * Updates a record.
	 *
	 * @param {String} path 	Path for API endpoint
	 * @param {Object} toUpdate Record tp update
	 * @return {Promise} 		Promise that resolves with the result
	 * @public
	 */
	update(path, toUpdate) {
		return this.iqmetrix.request(this.host, path, 'PUT', toUpdate);
	},
};

module.exports = base;
