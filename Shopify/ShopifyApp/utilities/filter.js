'use strict';

/**
 * example:
 * stringify({
 *      PrimaryName: 'John',
 *      DateOfBirth: new Date()
 *  });
 * Produces: PrimaryName eq 'John' and DateOfBirth eq '2016-07-06T20:11:05.486Z'
 */
exports.stringify = function (obj, sep, eq){
	var separator = sep || 'and';
	var equality = eq || 'eq';
	var result = '';
	var value;

	for (var property in obj) {
		if (obj.hasOwnProperty(property)) {
			if (result !== ''){
				result += ` ${separator} `;
			}
			value = JSON.stringify(obj[property]).replace(/"/g, "'");
			result += `${property} ${equality} ${value}`;
		}
	}
	return result;
}
