using MagentoConnect;
using MagentoConnect.Controllers.EndlessAisle;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Controllers.EndlessAisle
{
    /// <summary>
    /// This suite ensures the OrderController is working correctly
    /// </summary>
    [TestClass]
	public class OrderControllerTests
	{
        //IMPORTANT: Before you can run these tests, ensure the values below are replaced with ones from Endless Aisle
		private const string OrderId = "cdd26b8f-4ed1-409d-9984-982e081c425e";

        private OrdersController _orderController;

        [TestInitialize]
		public void SetUp()
		{
			var eaAuthToken = App.GetEaAuthToken();
			_orderController = new OrdersController(eaAuthToken);
		}

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
