﻿using System.Collections.Generic;
using System.Net;
using MagentoConnect.Models.EndlessAisle.Catalog;
using Newtonsoft.Json;
using RestSharp;

namespace MagentoConnect.Controllers.EndlessAisle
{
	public class CatalogsController : BaseController
	{
		public static string EndlessAisleAuthToken;

		public CatalogsController(string eaAuthToken)
		{
			EndlessAisleAuthToken = eaAuthToken;
		}

		/**
		 * Returns a CatalogItemResource representing a item in your Product Catalog
		 *
		 * @param   catalogItemId          Identifier for a CatalogItem to fetch
		 * @return  CatalogItemResource    Resource requested
		 */
		public CatalogItemResource GetCatalogItem(string catalogItemId)
		{        
			var endpoint = UrlFormatter.EndlessAisleGetCatalogUrl(catalogItemId);

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.GET);

			request.AddHeader("Authorization", string.Format("Bearer {0}", EndlessAisleAuthToken));
			request.AddHeader("Accept", "application/json");

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode);

			return JsonConvert.DeserializeObject<CatalogItemResource>(response.Content);
		}

		/// <summary>
		/// Gets the catalog items from the slug provided.
		/// </summary>
		/// <param name="slug">Slug to get items for.</param>
		/// <returns>Catalog items for the slug provided.</returns>
		public IEnumerable<CatalogItemResource> GetCatalogItemsBySlug(string slug)
		{
			var endpoint = UrlFormatter.EndlessAisleGetCatalogItemsBySlugUrl(slug);

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.GET);

			request.AddHeader("Authorization", string.Format("Bearer {0}", EndlessAisleAuthToken));
			request.AddHeader("Accept", "application/json");

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode);

			return JsonConvert.DeserializeObject<CatalogSearchResultResource>(response.Content).Items;
		}

		/**
		 * Deletes a Catalog Item 
		 *
		 * @param   catalogItemId   Identifier for a CatalogItem to delete
		 * @return  string          An empty string if sucessful
		 */
		public string DeleteCatalogItem(string catalogItemId)
		{
			var endpoint = UrlFormatter.EndlessAisleGetCatalogUrl(catalogItemId);

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.DELETE);

			request.AddHeader("Authorization", string.Format("Bearer {0}", EndlessAisleAuthToken));
			request.AddHeader("Accept", "application/json");
			request.AddHeader("Content-Type", "application/json");

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode, HttpStatusCode.NoContent);

			return response.Content;
		}

		/**
		 * Creates a Catalog Item 
		 *
		 * @param   CatalogItemResource       Object representing CatalogItem to be created
		 * @return  CatalogItemResource       CatalogItemResource that was created, if sucessful
		 */
		public CatalogItemResource CreateCatalogItem(CatalogItemResource catalogItem)
		{
			var endpoint = UrlFormatter.EndlessAisleCreateCatalogUrl();

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.POST);

			request.AddHeader("Authorization", string.Format("Bearer {0}", EndlessAisleAuthToken));
			request.AddHeader("Accept", "application/json");
			request.AddHeader("Content-Type", "application/json");

			request.AddJsonBody(catalogItem);

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode);

			return JsonConvert.DeserializeObject<CatalogItemResource>(response.Content);
		}

	}
}
