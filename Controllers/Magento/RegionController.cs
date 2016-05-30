using System;
using System.Collections.Generic;
using MagentoConnect.Models.Magento.Country;
using Newtonsoft.Json;
using RestSharp;

namespace MagentoConnect.Controllers.Magento
{
	public class RegionController : BaseController
	{
		public static string MagentoAuthToken;

		public RegionController(string magentoAuthToken)
		{
			MagentoAuthToken = magentoAuthToken;
		}

		/// <summary>
		/// Get the coutries and their region data from Magento
		/// </summary>
		/// <returns>List of coutries and their region data</returns>
		public IEnumerable<CountryResource> GetCountries()
		{
			var endpoint = UrlFormatter.MagentoGetCountriesUrl();

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.GET);

			request.AddHeader("Authorization", string.Format("Bearer {0}", MagentoAuthToken));
			request.AddHeader("Content-Type", "application/json");

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode);

			return JsonConvert.DeserializeObject<IEnumerable<CountryResource>>(response.Content);
		}
	}
}
