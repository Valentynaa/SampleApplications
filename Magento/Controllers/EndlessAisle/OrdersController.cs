using System;
using System.Collections.Generic;
using MagentoSync.Models.EndlessAisle.Orders;
using MagentoSync.Utilities;
using Newtonsoft.Json;
using RestSharp;

namespace MagentoSync.Controllers.EndlessAisle
{
	public class OrdersController : BaseController, IOrdersController
	{
		public string AuthToken { get; }
		public OrdersController(string eaAuthToken)
		{
			AuthToken = eaAuthToken;
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

			request.AddHeader("Authorization", string.Format("Bearer {0}", AuthToken));
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

			request.AddHeader("Authorization", string.Format("Bearer {0}", AuthToken));
			request.AddHeader("Accept", "application/json");

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode);

			return JsonConvert.DeserializeObject<IEnumerable<OrderItemResource>>(response.Content);
		}
	}
}
