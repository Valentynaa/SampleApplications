'use strict';

const webhook = require('../webhook');
const colors = require('colors/safe');
const ProductMapper = require('../../mappers/product-mapper');
const app = require('../../app');

/**
 * Handles a topic from a webhook. Sends the request to the product mapper function
 * that handles the provided action.
 *
 * @param {Object} req		Request made
 * @param {String} action	Action performed on topic (ex. create / update / delete)
 * @public
 */
exports.handleTopic = function (req, action){
	var funcName = action + 'Product';
	webhook.getStores().then(stores => {
		var pm = new ProductMapper(stores.iqmetrix, stores.shopify);
		var func = pm[funcName];
		if(typeof func === 'function'){
			func(req.body);
		}else{
			console.log(colors.red('Unable to handle webhook. No function exists for products/' + action));
		}
	});
}
