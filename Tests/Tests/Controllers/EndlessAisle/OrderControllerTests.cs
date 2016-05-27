using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagentoConnect;
using MagentoConnect.Controllers.EndlessAisle;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Controllers.EndlessAisle
{
	[TestClass]
	public class OrderControllerTests
	{
		//Private variables go here
		private OrdersController _orderController;
		private const string OrderId = "cdd26b8f-4ed1-409d-9984-982e081c425e";

		[TestInitialize]
		public void SetUp()
		{
			string eaAuthToken = App.GetEaAuthToken();
			_orderController = new OrdersController(eaAuthToken);
		}

		//Tests go here

		/// <summary>
		/// This test ensures the request does not error
		/// </summary>
		[TestMethod]
		public void OrderController_GetOrders()
		{
			Assert.IsNotNull(_orderController.GetOrders());
		}

		/// <summary>
		/// This test ensures the request does not error. If this test fails, either the OrderId provided is invalid or the request errored 
		/// </summary>
		[TestMethod]
		public void OrderController_GetOrderItems()
		{
			Assert.IsNotNull(_orderController.GetOrderItems(OrderId));
		}
	}
}
