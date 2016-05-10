using Newtonsoft.Json;
using RestSharp;
using AuthenticationCredentialsResource = MagentoConnect.Models.Magento.Authentication.AuthenticationCredentialsResource;

namespace MagentoConnect.Controllers.Magento
{
    public class AuthController : BaseController
    {
        private readonly string _authUrl;

        public AuthController()
        {
            _authUrl = UrlFormatter.MagentoAuthUrl();
        }

        /**
         * Returns an Authentication string used for accessing Magento APIs
         *
         * @param   AuthenticationCredentialsResource  A model containing authentication credentials
         * @return  string                             A string representing a token used for accessing Magento APIs
         */
        public string Authenticate(AuthenticationCredentialsResource credentials)
        {
            var client = new RestClient(_authUrl);
            var request = new RestRequest(Method.POST);

            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(credentials);

            var response = client.Execute(request);

            //Ensure we get the right code
            CheckStatusCode(response.StatusCode);

            return JsonConvert.DeserializeObject<string>(response.Content);
        }
    }
}