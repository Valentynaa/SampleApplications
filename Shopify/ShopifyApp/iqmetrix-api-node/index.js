'use strict';

const camelCase = require('lodash/camelCase');
const assign = require('lodash/assign');
const path = require('path');
const got = require('got');
const unirest = require('unirest');
const fs = require('fs');
const nconf = require('nconf');
const FormData = require('form-data');
const form = new FormData();
const querystring = require('querystring');
const colors = require('colors/safe');
const util = require('util');
const app = require('../app');

//var languageSpecificServices = ['productlibrary', 'ams'];

/**
 * Creates an iQmetrix instance.
 *
 * @param {String} key The API Key
 * @param {String} env iQmetrix Environment
 * @constructor
 * @public
 */
function iQmetrix(key, env) {
	if (!(this instanceof iQmetrix)) return new iQmetrix(key, env);

	this.token = key;
	this.environment = env;
	this.baseUrl = {
		environment: env,
		protocol: 'https:',
		auth: key
	};
}

/**
 * Sends a request to an iQmetrix API endpoint.
 *
 * @param {String} host 		Host of API endpoint
 * @param {String} path 		Path for endpoint
 * @param {String} method 		HTTP method
 * @param {Object} params 		Request body or query string if GET request (optional for GET or DELETE)
 * @param {String} filePath 	File system path to file to add to request body (optional)
 * @return {Promise}
 */
iQmetrix.prototype.request = function (host, path, method, params, filePath) {
	if(app.nconf.get('general:log_calls')){
		console.log('iQmetrix doing a ' + method + ' at ' + path);
	}
	
	if (filePath && method === 'POST'){
		return requestFile(host, path, method, filePath, this.baseUrl.protocol, this.token);
	} else {
		return request(host, path, method, params, this.baseUrl.protocol, this.token);
	}
};

function request(host, path, method, params, protocol, token){
	// host = 'b5617511.ngrok.io';
	// path = '';
	// protocol = 'http:'

	if (method == 'GET' && params) {
		path += '?' + querystring.stringify(params);
	}

	var headers = {
		'Content-Type': 'application/json',
		'Accept': 'application/json',
		'Authorization': `Bearer ${token}`,
		'Accept-Language': app.nconf.get('general:language_code')
	};

	var options = {
		protocol: protocol,
		host: host,
		path: path,
		method: method,
		headers: headers
  	};
		
	if (params) {
		const body = params;
		options.body = JSON.stringify(body);
	}
	
	return got(options).then(res => {
		const body = res.body;

		if (body){
			return JSON.parse(body);
		}
		return {};

	}, err => {
		console.log(colors.red(err));
		console.log(colors.red(err.response.body));
		//Uncomment line to see call options for replicating call in postman
		//console.log(util.inspect(options, {showHidden: false, depth: null}));
		return Promise.reject(err);
	});
}

function requestFile(host, path, method, filePath, protocol, token){
	var endpoint = `${protocol}//${host}${path}`;
	return new Promise(function(resolve, reject){
		unirest.post(endpoint)
		.headers({'Content-Type': 'multipart/form-data', 'Authorization': `Bearer ${token}`})  
		.field('Filename', fs.createReadStream(filePath)) // Form field
		.end(function (response) {
			resolve(JSON.parse(response.raw_body));
		});
	});
}

module.exports = iQmetrix;
