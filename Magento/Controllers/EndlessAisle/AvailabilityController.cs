using MagentoSync.Controllers.EndlessAisle.Interfaces;
using Newtonsoft.Json;
using RestSharp;
using MagentoSync.Models.EndlessAisle.Availability;

namespace MagentoSync.Controllers.EndlessAisle
{
	public class AvailabilityController : BaseController, IAvailabilityController
	{
		public string AuthToken { get; }

		public AvailabilityController(string eaAuthToken)
		{
			AuthToken = eaAuthToken;
		}

		/**
		 * Creates an Availability resource
		 *
		 * @param   AvailabilityResource       Object representing Availability to be created
		 * @return  AvailabilityResource       AvailabilityResource that was created, if sucessful
		 */
		public AvailabilityResource CreateCatalogItem(AvailabilityResource availability)
		{
			var endpoint = UrlFormatter.EndlessAisleCreateAvailabilityUrl();

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.POST);

			request.AddHeader("Authorization", string.Format("Bearer {0}", AuthToken));
			request.AddHeader("Accept", "application/json");
			request.AddHeader("Content-Type", "application/json");

			request.AddJsonBody(availability);

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode, System.Net.HttpStatusCode.Created);

			return JsonConvert.DeserializeObject<AvailabilityResource>(response.Content);
		}
	}
}
