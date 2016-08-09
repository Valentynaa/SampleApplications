'use strict';
const url = require('url');
const querystring = require('querystring');
const crypto = require('crypto');
const Shopify = require('shopify-api-node');
const colors = require('colors/safe');
const util = require('util');
const customers = require('./customers');
const products = require('./products');
const app = require('../../app');
const iqmetrixAuth = require('../iqmetrix_auth');
const iQmetrix = require('../../iqmetrix-api-node');

let topicHeader = 'x-shopify-topic';
let hashHeader = 'x-shopify-hmac-sha256';
let customerControllerName = 'customers';
let productControllerName = 'products';

/**
 * Handles a post request from a webhook. Responds with 200 if the request is verified
 * based on the hash value passed as a header. Responds with 401 if the POST is not 
 * vaerfied.
 *
 * @param {Object} req	Request made
 * @param {Object} res	Response to request
 * @public
 */
exports.handlePost = function(req, res){
	//console.log(util.inspect(req.headers, {showHidden: false, depth: null}));
	if (verifyShopifyHook(req)) {
		res.writeHead(200);
		res.end('Verified webhook');
		
		let logColor = 'green';
		if (req.headers[topicHeader].includes('delete')){
			logColor = 'red';
		}else if (req.headers[topicHeader].includes('update')){
			logColor = 'cyan';
		}else if (req.headers[topicHeader].includes('create')){
			logColor = 'yellow';
		}

		console.log('Verified webhook ' + colors[logColor](req.headers[topicHeader]));
		handleTopic(req, req.headers[topicHeader]);
	} else {
		res.writeHead(401);
		res.end('Unverified webhook');
		console.log('Unverified webhook ' + colors.red(req.headers[topicHeader]));
	}
}

/**
 * Returns an object with an iQmetrix and Shopify store instance. Stores are based on the shop name and 
 * access token for Shopify and the environment for iQmetrix
 *
 * @return {Promise} 	Promise that resolves with object contatining stores.
 * @public
 */
exports.getStores = function(){
	return new Promise(resolve => {
		let shopify = new Shopify(app.nconf.get('general:shop_name'), app.nconf.get('oauth:access_token'));
		iqmetrixAuth.getAccessToken().then(token => {
			let iqmetrix = new iQmetrix(token, app.nconf.get('iq_oauth:environment'));
			resolve({iqmetrix: iqmetrix, 
				shopify: shopify
			});
		});
	});
}

/**
 * Determines whether or not a request came from Shopify based on the hash in the headers
 *
 * @param {Object} req	Request made
 * @return {Boolean} 	Whether or not the request is verified.
 */
function verifyShopifyHook(req) {
	if (req.headers[hashHeader]){
		let digest = crypto.createHmac('SHA256', app.nconf.get('oauth:client_secret'))
		.update(new Buffer(req.rawBody), 'utf8')
		.digest('base64');
		return digest === req.headers[hashHeader];
	} else{
		return false;
	}
	
}

/**
 * Handles a topic from a webhook. Sends the request to the correct controller
 * to handle it.
 *
 * @param {Object} req		Request made
 * @param {String} topic	topic that request was made for
 */
function handleTopic(req, topic){
	let split = topic.split('/');
	let controller = split[0];
	let event = split[1];
	let func;

	switch(controller) {
		case customerControllerName:
			customers.handleTopic(req, event);
			break;
		case productControllerName:
			products.handleTopic(req, event);
			break;
		default:
			console.log(colors.red('Unable to handle webhook. No controller exists for ' + topic));
	}
}

exports.test = function(req, res){
	console.log(req.body);
	res.writeHead(200);
	res.end('Test');
}
