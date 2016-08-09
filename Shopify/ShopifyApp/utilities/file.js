'use strict';

const fs = require('fs');
const request = require('request');
const path = require('path');

/**
 * Downloads the content from a URI
 *
 * @param {String} uri 	URI to download from
 * @return {Promise} 	Promise that resolves with the filename
 * @public
 */
exports.download = function(uri){
	return new Promise((resolve, reject) => {
		let filename;
		if(uri.includes('shopify')){
			filename = fileNameFromShopifyUri(uri);
		}else{
			filename = path.basename(uri);
		}

		request.head(uri, function(err, res, body){
			request(uri).pipe(fs.createWriteStream(filename)).on('close', function(){
				resolve(filename);
			});
		});
	});
};

/**
 * Extracts the file name from a shopify URI
 *
 * @param {String} uri 	URI to get file name from 
 * @return {String} 	File name from URI
 * @public
 */
function fileNameFromShopifyUri(uri){
	let name = path.basename(uri);
	name = name.substring(0, name.indexOf('?'));
	return name;
}
