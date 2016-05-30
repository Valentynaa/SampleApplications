using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagentoConnect.Models.EndlessAisle.Orders;
using MagentoConnect.Utilities;
using Newtonsoft.Json;
using RestSharp;

namespace MagentoConnect.Controllers.EndlessAisle
{
	public class OrdersController : BaseController
	{
		public static string EndlessAisleAuthToken;
		public OrdersController(string eaAuthToken)
		{
			EndlessAisleAuthToken = eaAuthToken;
		}

		/// <summary>
		/// Gets all EA orders for the entire company based on the filter if one is used
		/// </summary>
		/// <param name="filter">filter criteria for orders</param>
		/// <returns>List of orders for the company</returns>
		public IEnumerable<OrderResource> GetOrders(Filter filter = null)
		{
			var endpoint = UrlFormatter.EndlessAisleGetOrdersUrl();

			if (filter != null)
				endpoint = UrlFormatter.HypermediaFilterUrl(endpoint, filter);

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
