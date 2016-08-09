'use-strict';

const bodyParser = require('body-parser');
const cookieParser = require('cookie-parser');
const cookieSession = require('cookie-session');
const express = require('express');
const path = require('path');			//Utility for dealing with file system paths
const nconf = require('nconf');			//Used for reading configuration values
const colors = require('colors/safe');	//Used to change colors of text in console
const Shopify = require('shopify-api-node');
const routes = require('./routes');
const iqmetrixAuth = require('./routes/iqmetrix_auth');
const shopifyAuth = require('./routes/shopify_auth');
const webhook = require('./routes/webhook');
const Customer = require('./iqmetrix-api-node/resources/customer');
const iQmetrix = require('./iqmetrix-api-node');
const CustomerMapper = require('./mappers/customer-mapper');
const ProductMapper = require('./mappers/product-mapper');

//Enable this block of code to see where console messages originate from
// var old = console.log;
// console.log = function() {
// 	var args = [].slice.apply(arguments).concat([(new Error()).stack.split(/\n/)[2].trim()]);
// 	return old.apply(this, args);
// };

//load settings from environment config
nconf.argv().env().file({
	file: (process.env.NODE_ENV || 'dev') + '-settings.json'
});
exports.nconf = nconf;
exports.rootDirectory = __dirname;

//Validate config settings
if(!IsConfigValid(nconf)){
	console.log(colors.red('Error in configuration file, ensure all fields are valid and filled out.'));
}

//configure express
var app = express();

//Add a rawBody to the req that can be buffered
//See: http://stackoverflow.com/questions/21998980/middleware-for-saving-raw-post-data-in-the-request-object-wont-next-and-cause
app.use(function(req, res, next) {
	req.rawBody = '';
	req.on('data', function(chunk) { 
		req.rawBody += chunk;
	});
	
	next();
});

//support json and url encoded requests
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: false }));

//setup encrypted session cookies
app.use(cookieParser());
app.use(cookieSession({
	secret: "--express-session-encryption-key--"
}));

//statically serve from the 'public' folder
app.use(express.static(path.join(__dirname, nconf.get('general:serve_location'))));

//use jade templating engine for view rendering
app.set('view engine', 'jade');

//use the environment's port if specified
app.set('port', process.env.PORT || 3000);
var appAuth = new shopifyAuth.AppAuth();


//Test Sync Block
// var iqmetrix;
// var customer;
// let syncTime = new Date('2000-01-01T16:29:12-04:00');
// var shopify = new Shopify(nconf.get('general:shop_name'), nconf.get('oauth:access_token'));
// iqmetrixAuth.getAccessToken().then(token => {
// 	console.log(token);
// 	iqmetrix = new iQmetrix(token, nconf.get('iq_oauth:environment'));
// 	cm = new CustomerMapper(iqmetrix, shopify);
// 	pm = new ProductMapper(iqmetrix, shopify);
// 
// 	cm.syncCustomers(syncTime);
// 	pm.syncProducts(syncTime);
// });

//configure routes
app.get('/', routes.index);
app.get('/auth_app', appAuth.initAuth);
app.get('/escape_iframe', appAuth.escapeIframe);
app.get('/auth_code', appAuth.getCode);
app.get('/auth_token', appAuth.getAccessToken);
app.get('/render_app', routes.renderApp);
app.post('/webhook', webhook.handlePost);

//Use for testing webhook calls
app.post('/', webhook.test);

var server = app.listen(app.get('port'), function() {
	console.log(colors.green(`Listening at port ${server.address().port}`));
});

function IsConfigValid (nconf){
	var general = nconf.get('general');
	var oauth = nconf.get('oauth');
	var iq_oauth = nconf.get('iq_oauth');
	var iq_settings = nconf.get('iq_settings');
	return general && general.title && general.shop_name && general.language_code && general.serve_location &&
		oauth && oauth.api_key && oauth.client_secret && oauth.redirect_url && oauth.access_token && oauth.scope &&
		iq_oauth && iq_oauth.grant_type && iq_oauth.client_id && iq_oauth.client_secret && iq_oauth.username && iq_oauth.password && iq_oauth.environment &&
		iq_settings && iq_settings.companyId && iq_settings.locationId && iq_settings.mappingFieldName && iq_settings.addressType && iq_settings.customerType && iq_settings.customerExtensionType
}

