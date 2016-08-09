using RestSharp;
using System;
using System.Configuration;
using System.Net;

namespace WebhookUpdater.Controllers
{
	public class BaseController
	{
		/// <summary>
		/// Sends a REST request to an endpoint and returns the response
		/// </summary>
		/// <param name="url">Endpoint for request</param>
		/// <param name="method">Request method ex. POST, GET, UPDATE, DELETE </param>
		/// <param name="parameters">Body of request</param>
		/// <returns>Response for request</returns>
		public IRestResponse Request(string url, Method method, object parameters = null)
		{
			var client = new RestClient(url);
			var request = new RestRequest(method);
			request.AddHeader("content-type", "application/json");
			request.AddHeader("x-shopify-access-token", ConfigurationManager.AppSettings["AccessToken"]);

			request.AddJsonBody(parameters);
			IRestResponse response = client.Execute(request);

			CheckStatusCode(response.StatusCode, method == Method.POST ? HttpStatusCode.Created : HttpStatusCode.OK);

			return response;
		}
			

		/// <summary>
		/// This function allows you to run CheckStatusCode without an expected code, it overrides with 200-OK
		/// </summary>
		/// <param name="actualCode"> code returned </param>
		protected void CheckStatusCode(HttpStatusCode actualCode)
		{
			CheckStatusCode(actualCode, HttpStatusCode.OK);
		}

		/// <summary>
		/// Allows you to check that the code returned was the one expected
		/// If an unexpected code was returned, it throws the exception wrapped in a message
		/// </summary>
		/// <param name="actualCode">HTTP code returned</param>
		/// <param name="expectedCode">Expected HTTP status code</param>
		protected void CheckStatusCode(HttpStatusCode actualCode, HttpStatusCode expectedCode)
		{
			if (actualCode != expectedCode)
			{
				throw new Exception(string.Format("Unexpected HTTP status code. Expected {0}, returned {1}", expectedCode, actualCode));
			}
		}
	}
}