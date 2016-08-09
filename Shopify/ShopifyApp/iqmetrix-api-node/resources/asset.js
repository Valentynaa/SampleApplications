'use strict';
var app = require('../../app');
const assign = require('lodash/assign');
const nconf = require('nconf');
const base = require('../mixins/base');
const path = require('path');

var appDir = path.dirname(require.main.filename);

/**
 * Creates an Asset instance.
 *
 * @param {iQmetrix} iqmetrix 	Reference to iQmetrix instance
 * @constructor
 * @public
 */
function Asset(iqmetrix) {
	this.iqmetrix = iqmetrix;
	this.service = 'ams';
	this.host = `${this.service}${this.iqmetrix.baseUrl.environment}.iqmetrix.net`;
}
assign(Asset.prototype, base);

/**
 * Creates an Asset record.
 *
 * @param {String} fileName 	Name of file in root directory to set as Asset
 * @return {Promise} 			Promise that resolves with the result
 * @public
 */
Asset.prototype.createAsset = function (fileName) {
	var path = `/assets`;
	var filePath = app.rootDirectory + '\\' + fileName;
	return this.create(path, undefined, filePath);
};

/**
 * Returns an Asset
 *
 * @param {String} assetId 	ID value for asset to retrieve
 * @return {Promise} 		Promise that resolves with the result
 * @public
 */
Asset.prototype.retrieveAsset = function (assetId){
	var path = `/assets/${assetId}`;
	return this.retrieve(path);
}

module.exports = Asset;
