using System.Collections.Generic;
using MagentoSync.Controllers.Magento.Interfaces;
using MagentoSync.Models.Magento.Country;
using Newtonsoft.Json;
using RestSharp;

namespace MagentoSync.Controllers.Magento
{
	public class RegionController : BaseController, IRegionController
	{
		public string AuthToken { get; }

		public RegionController(string magentoAuthToken)
		{
			AuthToken = magentoAuthToken;
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

			request.AddHeader("Authorization", string.Format("Bearer {0}", AuthToken));
			request.AddHeader("Content-Type", "application/json");

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode);

			return JsonConvert.DeserializeObject<IEnumerable<CountryResource>>(response.Content);
		}
	}
}
