# Webhook Updater

This project is intended to accompany the [iQmetrix Shopify integration](https://github.com/iQmetrix/SampleApplications/tree/master/Shopify/ShopifyApp/) to provide an easy way to set up and adjust [webhooks](https://help.shopify.com/api/reference/webhook) for a Shopify store.

The alternative to this tool is to manually set up all webhooks via Postman using [this collection.](https://www.getpostman.com/collections/9394f607797426968812)

## Table Of Contents

* [Getting Started](#getting-started)
* [How it Works](#how-it-works)
* [Limitations](#limitations)
	* [Webhook Updates](#webhook-updates)
	* [Problem Topics](#problem-topics)
* [File Structure](#file-structure)

## Getting Started

To get started ensure that the following are installed on your computer:

 * Visual Studio 2012 or newer (**NOTE**: this project may be incompatible with older versions of VS)
 * [Nuget](https://www.nuget.org/) package management solution
 * [RestSharp](http://restsharp.org/) REST and HTTP API client for .Net
 * [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json/) Nuget package for working with JSON 

Next, fill in the Shopify store name (as it appears in a URL) and Access Token to give the application the parameters required to make the API calls. These fields are both filled in the `App.config` file.

Additionally, if you use a namespace other than the default namespace (`"global"`) for the Shopify App mapping fields, then you will also need to adjust the `MetafieldNamespace` field.

## How it Works

The Webhook Updater works through performing the following steps in sequence.

* Display the configuration to ensure a correct setup is used.
* Retrieve a list of all existing webhooks for the store provided.
* Determine an endpoint for new webhooks to be created for based on user input. (**NOTE**: Do not forget to add `/webhook` to the endpoint that the Shopify Application is using if that is the purpose of using this tool)
    * Determine the topics to create webhooks for by parsing a file selected by the user.
    * Each line of the file will be read and used as a topic. ex. `customers/update`
    * Create webhooks based on topics and endpoint gathered.
* Determine an endpoint for webhooks to be deleted for based on user input.
    * Use the list of existing webhooks to determine matches to the endpoint and delete them.

## Limitations

This tool is intended for development use only, and has multiple limitations:

### Webhook Updates

This application only creates and deletes webhooks for the specified store. Although in some cases a webhook update could be used to provide the desired effect, this application does not do so.

### Problem Topics

Certain topics return errors upon being created by the application. Below is a list of all topics that have been found to return errors:
 
* products/create

To create a webhook for any of the above topics, use [this Postman collection](https://www.getpostman.com/collections/9394f607797426968812) to manually create the webhook.

## File Structure

The files in the Webhook Updater project are organized in the following structure:

* Controllers - Classes that deal specifically with sending API requests
* Resources - Transport objects, or classes representing resources
* Properties - Visual Studio project information
* Utilities - Useful utility classes
* App.config - Configuration file to hold constants and authentication credentials
* Program.cs - Main console application file
* packages.config - Packages file describing nuget packages, see [Nuget](https://www.nuget.org/)
