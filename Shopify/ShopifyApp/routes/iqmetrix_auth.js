'use strict';

var app = require('../app'),
	request = require('request');

/**
 * Gets an auth token from iQmetrix based on the auth credentials in
 * dev-settings.json.
 *
 * @return {Promise} 	Promise that resolves with the Access Token
 */
exports.getAccessToken = function() {
	return new Promise(resolve => {
		var environment = app.nconf.get('iq_oauth:environment');
		var options = { 
			method: 'POST',
			url: `https://accounts${environment}.iqmetrix.net/v1/oauth2/token`,
			headers: { 
				'content-type': 'application/x-www-form-urlencoded' 
			},
			form: { 
				grant_type: app.nconf.get('iq_oauth:grant_type'),
				client_id: app.nconf.get('iq_oauth:client_id'),
				client_secret: app.nconf.get('iq_oauth:client_secret'),
				username: app.nconf.get('iq_oauth:username'),
				password: app.nconf.get('iq_oauth:password') 
			} 
		};

		request(options, function (error, response, body) {
			body = JSON.parse(body);
			if (error)
				throw new Error(error);
			resolve(body['access_token']);
		});
	})
}
