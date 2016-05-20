using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagentoConnect.Models.EndlessAisle.Order;
using Newtonsoft.Json;
using RestSharp;

namespace MagentoConnect.Controllers.EndlessAisle
{
	public class OrderController : BaseController
	{
		public static string EndlessAisleAuthToken;
		public OrderController(string eaAuthToken)
		{
			EndlessAisleAuthToken = eaAuthToken;
		}

		/// <summary>
		/// Gets all EA orders for the entire company 
		/// </summary>
		/// <returns>List of orders for the company</returns>
		public IEnumerable<OrderResource> GetOrders()
		{
			var endpoint = UrlFormatter.EndlessAisleGetOrdersUrl();

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.GET);

			request.AddHeader("Authorization", string.Format("Bearer {0}", EndlessAisleAuthToken));
			request.AddHeader("Accept", "application/json");

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode);

			return JsonConvert.DeserializeObject<IEnumerable<OrderResource>>(response.Content);
		}

		/// <summary>
		/// Gets the items that appear on a specified order
		/// </summary>
		/// <param name="orderId">Order to get items for</param>
		/// <returns>List of items in the order</returns>
		public IEnumerable<OrderItemResource> GetOrderItems(string orderId)
		{
			var endpoint = UrlFormatter.EndlessAisleGetOrderItemsUrl(orderId);

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.GET);

			request.AddHeader("Authorization", string.Format("Bearer {0}", EndlessAisleAuthToken));
			request.AddHeader("Accept", "application/json");

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode);

			return JsonConvert.DeserializeObject<IEnumerable<OrderItemResource>>(response.Content);
		}
	}
}
