using System.Collections.Generic;
using MagentoConnect.Models.EndlessAisle.Orders;
using MagentoConnect.Utilities;

namespace MagentoConnect.Controllers.EndlessAisle
{
	public interface IOrdersController : IController
	{
		/// <summary>
		/// Gets all EA orders for the entire company based on the filter if one is used
		/// </summary>
		/// <param name="filter">filter criteria for orders</param>
		/// <returns>List of orders for the company</returns>
		IEnumerable<OrderResource> GetOrders(Filter filter = null);

		/// <summary>
		/// Gets the items that appear on a specified order
		/// </summary>
		/// <param name="orderId">Order to get items for</param>
		/// <returns>List of items in the order</returns>
		IEnumerable<OrderItemResource> GetOrderItems(string orderId);
	}
}