﻿using System.Collections.Generic;
using System.Linq;
using MagentoConnect.Models.EndlessAisle.Entities;
using Newtonsoft.Json;
using RestSharp;

namespace MagentoConnect.Controllers.EndlessAisle
{
	public class EntitiesController : BaseController
	{
		public static string EndlessAisleAuthToken;

		public EntitiesController(string eaAuthToken)
		{
			EndlessAisleAuthToken = eaAuthToken;
		}

		/// <summary>
		/// Gets information about all manufacturers
		/// </summary>
		/// <returns>Returns a list of manufacturers</returns>
		public List<ManufacturerResource> GetAllManufacturers()
		{
			var endpoint = UrlFormatter.EndlessAisleEntitiesManufacturersUrl();

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.GET);

			request.AddHeader("Authorization", string.Format("Bearer {0}", EndlessAisleAuthToken));
			request.AddHeader("Accept", "application/json");

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode);

			return JsonConvert.DeserializeObject<List<ManufacturerResource>>(response.Content);
		}

		/// <summary>
		/// Gets the EA location set in the App.config file
		/// </summary>
		/// <returns></returns>
		public LocationResource GetLocation()
		{
			var endpoint = UrlFormatter.EndlessAisleGetLocationUrl();

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.GET);

			request.AddHeader("Authorization", string.Format("Bearer {0}", EndlessAisleAuthToken));
			request.AddHeader("Accept", "application/json");

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode);

			return JsonConvert.DeserializeObject<LocationResource>(response.Content);
		}
	}
}
