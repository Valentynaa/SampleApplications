using MagentoConnect.Models.EndlessAisle.Authentication;
using Newtonsoft.Json;
using RestSharp;

namespace MagentoConnect.Controllers.EndlessAisle
{
    public class AuthController : BaseController
    {
        private readonly string _authUrl;

        public AuthController()
        {
            _authUrl = UrlFormatter.EndlessAisleAuthUrl();
        }

        /**
         * Returns an Authentication string used for accessing EA APIs
         *
         * @param   AuthenticationCredentials   A model containing authentication credentials
         * @return  access_token                A string representing a token used for accessing EA APIs
         */
        public string Authenticate(AuthenticationCredentialsResource credentials)
        {
            var client = new RestClient(_authUrl);
            var request =
                new RestRequest("token", Method.POST)
                    .AddParameter("grant_type", credentials.grant_type)
                    .AddParameter("username", credentials.username)
                    .AddParameter("password", credentials.password)
                    .AddParameter("client_id", credentials.client_id)
                    .AddParameter("client_secret", credentials.client_secret);

            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            
            var response = client.Execute(request);

            //Ensure we get the right code
            CheckStatusCode(response.StatusCode);

            return JsonConvert.DeserializeObject<AuthTokenResource>(response.Content).access_token;
        }
    }
}
