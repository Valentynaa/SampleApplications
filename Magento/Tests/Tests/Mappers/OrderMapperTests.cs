using System;
using System.Linq;
using MagentoSync;
using MagentoSync.Mappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.MockObjects.Controllers.EndlessAisle;
using Tests.MockObjects.Controllers.Magento;

namespace Tests.Mappers
{

	/// <summary>
	/// This suite ensures the OrderMapper is working correctly
	/// 
	/// NOTE:
	///		This class does NOT use actual calls to the APIs and instead relies on mock controllers
	/// </summary>
	[TestClass]
	public class OrderMapperTests
	{
		private readonly int _cartId = MockCartController.CartId;

		private OrderMapper _orderMapper;
		private CustomerMapper _customerMapper;
		private EntityMapper _entityMapper;
		private int _orderItemsCount;

		[TestInitialize]
		public void SetUp()
		{
			_orderMapper = new OrderMapper(new MockOrdersController(), new MockCatalogsController(), new MockCartController(), new MockProductController());
			_customerMapper = new CustomerMapper(new MockCustomerController());
			_entityMapper = new EntityMapper(new MockEntitiesController(), new MockRegionController());
			_orderItemsCount = MockOrdersController.OrderItemCount;
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
			_orderMapper.AddOrderItemsToCart(new Guid().ToString(), _cartId);
		}

		/// <summary>
		/// This test ensures that an exception is thrown for an invalid order ID
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void OrderMapper_AddOrderItemsToCart_InvalidOrderId()
		{
			_orderMapper.AddOrderItemsToCart("xxx", _cartId);
		}

		/// <summary>
		/// This test ensures that the orders returned are in the correct time frame
		/// </summary>
		[TestMethod]
		public void OrderMapper_GetEaOrdersCreatedAfter()
		{
			var orders = _orderMapper.GetEaOrdersCreatedAfter(DateTime.MinValue).ToList();
			Assert.AreEqual(_orderItemsCount, orders.Count);
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
			_orderMapper.SetShippingAndBillingInformationForCart(_cartId, _entityMapper.MagentoRegion, _entityMapper.EaLocation, _customerMapper.MagentoCustomer);
		}
		
		/// <summary>
		/// This test ensures that an exception is thrown for an invalid order ID
		/// </summary>
		[TestMethod]
		public void OrderMapper_CreateOrderForCart()
		{
			var cartIdForOrder = _orderMapper.CreateCustomerCart();

			_orderMapper.AddOrderItemsToCart(new Guid().ToString(), cartIdForOrder);
			_orderMapper.SetShippingAndBillingInformationForCart(cartIdForOrder, _entityMapper.MagentoRegion, _entityMapper.EaLocation, _customerMapper.MagentoCustomer);
			_orderMapper.CreateOrderForCart(cartIdForOrder);
		}
	}
}
