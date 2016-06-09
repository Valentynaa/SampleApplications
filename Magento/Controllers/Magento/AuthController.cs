using MagentoSync.Models;
using Newtonsoft.Json;
using RestSharp;

namespace MagentoSync.Controllers.Magento
{
	public class AuthController : BaseController, IAuthController
	{
		public string AuthToken { get; private set; }
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
		public string Authenticate(IAuthenticationCredentials credentials)
		{
			var client = new RestClient(_authUrl);
			var request = new RestRequest(Method.POST);

			request.AddHeader("Content-Type", "application/json");
			request.AddJsonBody(credentials);

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode);

			AuthToken = JsonConvert.DeserializeObject<string>(response.Content);
			return AuthToken;
		}
	}
}