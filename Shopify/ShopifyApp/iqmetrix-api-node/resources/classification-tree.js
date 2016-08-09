'use strict';
var app = require('../../app');
const assign = require('lodash/assign');
const nconf = require('nconf');
const base = require('../mixins/base');

/**
 * Creates a ClassificationTree instance.
 *
 * @param {iQmetrix} iqmetrix 	Reference to iQmetrix instance
 * @constructor
 * @public
 */
function ClassificationTree(iqmetrix) {
	this.iqmetrix = iqmetrix;
	this.service = 'productlibrary';
	this.host = `${this.service}${this.iqmetrix.baseUrl.environment}.iqmetrix.net`;
}
assign(ClassificationTree.prototype, base);

/**
 * Returns a Classification Tree
 *
 * @param {Number} classificationTreeId 	ID value for tree to retrieve
 * @return {Promise} 						Promise that resolves with the result
 * @public
 */
ClassificationTree.prototype.retrieveClassificationTree = function (classificationTreeId) {
	var path = (`/v1/ClassificationTrees(${classificationTreeId})`);
	return this.retrieve(path);
};

module.exports = ClassificationTree;
