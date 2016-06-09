using System;
using MagentoSync.Models.Magento.Customer;
using Newtonsoft.Json;
using RestSharp;

namespace MagentoSync.Controllers.Magento
{
	public class CustomerController : BaseController, ICustomerController
	{
		public string AuthToken { get; }

		public CustomerController(string magentoAuthToken)
		{
			AuthToken = magentoAuthToken;
		}

		/// <summary>
		/// Gets the Magento Customer specified
		/// </summary>
		/// <param name="customerId">Customer identifier</param>
		/// <returns>Magento Customer</returns>
		public CustomerResource GetCustomer(int customerId)
		{
			var endpoint = UrlFormatter.MagentoGetCustomerByIdUrl(customerId);

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.GET);

			request.AddHeader("Authorization", string.Format("Bearer {0}", AuthToken));
			request.AddHeader("Content-Type", "application/json");

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode);

			return JsonConvert.DeserializeObject<CustomerResource>(response.Content);
		}
	}
}
