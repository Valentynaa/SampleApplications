using Newtonsoft.Json;
using RestSharp;
using MagentoConnect.Models.EndlessAisle.Availability;

namespace MagentoConnect.Controllers.EndlessAisle
{
    public class AvailabilityController : BaseController
    {
        public static string EndlessAisleAuthToken;

        public AvailabilityController(string eaAuthToken)
        {
            EndlessAisleAuthToken = eaAuthToken;
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

            request.AddHeader("Authorization", string.Format("Bearer {0}", EndlessAisleAuthToken));
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
