using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagentoConnect.Models.Magento.Customer;
using Newtonsoft.Json;
using RestSharp;

namespace MagentoConnect.Controllers.Magento
{
	public class CustomerController : BaseController
	{
		public static string MagentoAuthToken;

		public CustomerController(string magentoAuthToken)
		{
			MagentoAuthToken = magentoAuthToken;
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

			request.AddHeader("Authorization", string.Format("Bearer {0}", MagentoAuthToken));
			request.AddHeader("Content-Type", "application/json");

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode);

			return JsonConvert.DeserializeObject<CustomerResource>(response.Content);
		}
	}
}
