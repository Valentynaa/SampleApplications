using System.Collections.Generic;
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

        /**
         * Gets information about all manufacturers
         *
         * @return  List<ManufacturerResource>     Returns a list of manufacturers
         */
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
    }
}
