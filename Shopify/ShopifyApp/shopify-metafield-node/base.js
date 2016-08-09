'use strict';

const assign = require('lodash/assign');
const qs = require('qs');

/**
 * This provides methods used by resources that have no relationships with
 * other resources. It's not meant to be used directly.
 *
 * @mixin
 */
const base = {
  /**
   * Counts the number of records.
   *
   * @param {Object} params 	Query parameters
   * @return {Promise} Promise that resolves with the result
   * @public
   */
  count(params) {
    const key = 'count';
    const url = this.buildUrl(key, params);
    return this.shopify.request(url, 'GET', key);
  },

  /**
   * Builds the request URL.
   *
   * @param {String} resource 	Resource type
   * @param {Number|String} id 	Record ID for resource
   * @param {Object} query 		Query parameters
   * @return {Object} URL object
   * @private
   */
  buildUrl(resource, id, query) {
    id || id === 0 || (id = '');

    let path = `/admin/${resource}/${id}/metafields`
      .replace(/\/+/g, '/')
      .replace(/\/$/, '');

    path += '.json';
    if (query) path += '?' + qs.stringify(query, { arrayFormat: 'brackets' });

    return assign({ path }, this.shopify.baseUrl);
  }
};

module.exports = base;
