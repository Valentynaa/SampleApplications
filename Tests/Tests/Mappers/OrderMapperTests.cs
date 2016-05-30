using System;
using System.Linq;
using MagentoConnect;
using MagentoConnect.Mappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Mappers
{

    /// <summary>
    /// This suite ensures the OrderMapper is working correctly
    /// </summary>
    [TestClass]
	public class OrderMapperTests
	{
        //IMPORTANT: Before you can run these tests, ensure the values below are replaced with ones from your Magento system
        private const int CartId = 3;

        //IMPORTANT: Before you can run these tests, ensure the values below are replaced with ones from Endless Aisle
        private const string OrderId = "2f471f62-411a-412a-89c9-5a5f4d9184be";

        private OrderMapper _orderMapper;
		private CustomerMapper _customerMapper;
		private EntityMapper _entityMapper;
		private DateTime _filterDate;

		[TestInitialize]
		public void SetUp()
		{
			var eaAuthToken = App.GetEaAuthToken();
			var magentoAuthToken = App.GetMagentoAuthToken();
			_orderMapper = new OrderMapper(magentoAuthToken, eaAuthToken);
			_customerMapper = new CustomerMapper(magentoAuthToken, eaAuthToken);
			_entityMapper = new EntityMapper(magentoAuthToken, eaAuthToken);
			_filterDate = new DateTime(2015, 12, 13);
		}

		/// <summary>
		/// This test ensures that a customer cart can be created
		/// </summary>
		[TestMethod]
		public void OrderMapper_CreateCustomerCart()
		{
			_orderMapper.CreateCustomerCart();
		}

		/// <summary>
		/// This test ensures that order items can be added to a cart for a valid cart and order
		/// </summary>
		[TestMethod]
		public void OrderMapper_AddOrderItemsToCart()
		{
			_orderMapper.AddOrderItemsToCart(OrderId, CartId);
		}

		/// <summary>
		/// This test ensures that an exception is thrown for an invalid order ID
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void OrderMapper_AddOrderItemsToCart_InvalidOrderId()
		{
			_orderMapper.AddOrderItemsToCart("xxx", CartId);
		}

		/// <summary>
		/// This test ensures that the orders returned are in the correct time frame
		/// </summary>
		[TestMethod]
		public void OrderMapper_GetEaOrdersCreatedAfter()
		{
			var ordersAllTime = _orderMapper.GetEaOrdersCreatedAfter(DateTime.MinValue).ToList();
			var ordersFiltered = _orderMapper.GetEaOrdersCreatedAfter(_filterDate).ToList();
			Assert.IsTrue(ordersFiltered.Count < ordersAllTime.Count);
			Assert.AreEqual(true, !ordersFiltered.Any(x => x.CreatedDateUtc < _filterDate));
		}

		/// <summary>
		/// This test ensures that an exception occurs when a future date is specified to get orders from
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void OrderMapper_GetEaOrdersCreatedAfter_InvalidTime()
		{
			_orderMapper.GetEaOrdersCreatedAfter(DateTime.MaxValue);
		}

		/// <summary>
		/// This test ensures that a cart can have shipping information set on it
		/// </summary>
		[TestMethod]
		public void OrderMapper_SetShippingAndBillingInformationForCart()
		{
			_orderMapper.SetShippingAndBillingInformationForCart(CartId, _entityMapper.MagentoRegion, _entityMapper.EaLocation, _customerMapper.MagentoCustomer);
		}

		/// <summary>
		/// This test ensures that an exception occurs when a cart unable to have shipping information set is selected
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void OrderMapper_SetShippingAndBillingInformationForCart_InvalidCart()
		{
			_orderMapper.SetShippingAndBillingInformationForCart(1, _entityMapper.MagentoRegion, _entityMapper.EaLocation, _customerMapper.MagentoCustomer);
		}

		/// <summary>
		/// This test ensures that an exception is thrown for an invalid order ID
		/// </summary>
		[TestMethod]
		public void OrderMapper_CreateOrderForCart()
		{
			var cartIdForOrder = _orderMapper.CreateCustomerCart();

			_orderMapper.AddOrderItemsToCart(OrderId, cartIdForOrder);
			_orderMapper.SetShippingAndBillingInformationForCart(cartIdForOrder, _entityMapper.MagentoRegion, _entityMapper.EaLocation, _customerMapper.MagentoCustomer);
			_orderMapper.CreateOrderForCart(cartIdForOrder);
		}
	}
}
