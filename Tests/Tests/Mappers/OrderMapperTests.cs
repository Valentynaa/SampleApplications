using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagentoConnect;
using MagentoConnect.Mappers;
using MagentoConnect.Models.EndlessAisle.Entities;
using MagentoConnect.Models.EndlessAisle.Order;
using MagentoConnect.Models.Magento.Country;
using MagentoConnect.Models.Magento.Customer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Mappers
{
	[TestClass]
	public class OrderMapperTests
	{
		//Private variables go here
		private OrderMapper _orderMapper;
		private CustomerMapper _customerMapper;
		private DateTime _filterDate;
		private const int CartId = 3;
		private const string OrderId = "2f471f62-411a-412a-89c9-5a5f4d9184be";

		[TestInitialize]
		public void SetUp()
		{
			string eaAuthToken = App.GetEaAuthToken();
			string magentoAuthToken = App.GetMagentoAuthToken();
			_orderMapper = new OrderMapper(magentoAuthToken, eaAuthToken);
			_customerMapper = new CustomerMapper(magentoAuthToken, eaAuthToken);
			_filterDate = new DateTime(2015, 12, 13);
		}

		//Tests go here

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
			var orders = _orderMapper.GetEaOrdersCreatedAfter(DateTime.MaxValue);
		}

		/// <summary>
		/// This test ensures that a cart can have shipping information set on it
		/// </summary>
		[TestMethod]
		public void OrderMapper_SetShippingAndBillingInformationForCart()
		{
			_orderMapper.SetShippingAndBillingInformationForCart(CartId, _customerMapper.MagentoCustomer);
		}

		/// <summary>
		/// This test ensures that an exception occurs when a cart unable to have shipping information set is selected
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void OrderMapper_SetShippingAndBillingInformationForCart_InvalidCart()
		{
			_orderMapper.SetShippingAndBillingInformationForCart(1, _customerMapper.MagentoCustomer);
		}

		/// <summary>
		/// This test ensures that an exception is thrown for an invalid order ID
		/// </summary>
		[TestMethod]
		public void OrderMapper_CreateOrderForCart()
		{
			int cartIdForOrder = _orderMapper.CreateCustomerCart();

			_orderMapper.AddOrderItemsToCart(OrderId, cartIdForOrder);
			_orderMapper.SetShippingAndBillingInformationForCart(cartIdForOrder, _customerMapper.MagentoCustomer);
			_orderMapper.CreateOrderForCart(cartIdForOrder);
		}
	}
}
