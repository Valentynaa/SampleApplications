'use strict';

const webhook = require('../webhook');
const colors = require('colors/safe');
const CustomerMapper = require('../../mappers/customer-mapper');
const app = require('../../app');

/**
 * Handles a topic from a webhook. Sends the request to the customer mapper function
 * that handles the provided action.
 *
 * @param {Object} req		Request made
 * @param {String} action	Action performed on topic (ex. create / update / delete)
 * @public
 */
exports.handleTopic = function (req, action){
	var funcName = action + 'Customer';
	webhook.getStores().then(stores => {
		var cm = new CustomerMapper(stores.iqmetrix, stores.shopify);
		var func = cm[funcName];
		if(typeof func === 'function'){
			func(req.body);
		}else{
			console.log(colors.red('Unable to handle webhook. No function exists for customers/' + action));
		}
	});
}
