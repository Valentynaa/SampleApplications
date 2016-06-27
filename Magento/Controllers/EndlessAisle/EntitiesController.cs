using System.Collections.Generic;
using System.Linq;
using MagentoSync.Controllers.EndlessAisle.Interfaces;
using MagentoSync.Models.EndlessAisle.Entities;
using Newtonsoft.Json;
using RestSharp;

namespace MagentoSync.Controllers.EndlessAisle
{
	public class EntitiesController : BaseController, IEntitiesController
	{
		public string AuthToken { get; }

		public EntitiesController(string eaAuthToken)
		{
			AuthToken = eaAuthToken;
		}

		/// <summary>
		/// Gets information about all supported manufacturers in EA
		/// </summary>
		/// <returns>List of supported Manufacturers in EA</returns>
		public List<ManufacturerResource> GetAllManufacturers()
		{
			var endpoint = UrlFormatter.EndlessAisleEntitiesManufacturersUrl();

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.GET);

			request.AddHeader("Authorization", string.Format("Bearer {0}", AuthToken));
			request.AddHeader("Accept", "application/json");

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode);

			return JsonConvert.DeserializeObject<List<ManufacturerResource>>(response.Content);
		}

		/// <summary>
		/// Gets the EA location the device is at (from App.config)
		/// </summary>
		/// <returns>EA Location device is at</returns>
		public LocationResource GetLocation()
		{
			var endpoint = UrlFormatter.EndlessAisleGetLocationUrl();

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.GET);

			request.AddHeader("Authorization", string.Format("Bearer {0}", AuthToken));
			request.AddHeader("Accept", "application/json");

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode);

			return JsonConvert.DeserializeObject<LocationResource>(response.Content);
		}
	}
}
