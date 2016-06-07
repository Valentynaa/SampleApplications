using System;
using System.Collections.Generic;
using System.Linq;
using MagentoConnect;
using MagentoConnect.Controllers.Magento;
using MagentoConnect.Models.Magento.Cart;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Controllers.Magento
{
	/// <summary>
	/// This suite ensures the CartController is working correctly
	/// </summary>
	[TestClass]
	public class CartControllerTests
	{
		//IMPORTANT: Before you can run these tests, ensure the values below are replaced with ones from your Magento system
		private const int CartId = 3;
		private const string ItemSku = "Configurable Product";
		private const string PaymentMethod = "checkmo";
		private const string ShippingMethod = "flatrate";
		private const int CustomerId = 2;
		private const int SaskRegionId = 77;

		private CartController _cartController;
		private CartAddItemResource _itemToAdd;
		private CartAddPaymentMethodResource _methodToAdd;
		private CartSetShippingInformationResource _shippingToSet;
		private AddressResource _address;

		/// <summary>
		/// NOTE:
		///		Make sure you have enough stock of the item used for testing in magento or tests may fail
		/// </summary>
		[TestInitialize]
		public void SetUp()
		{
			var magentoAuthToken = App.GetMagentoAuthToken();
			_cartController = new CartController(magentoAuthToken);

			_itemToAdd = new CartAddItemResource(CartId, ItemSku, 1);

			_methodToAdd = new CartAddPaymentMethodResource(CartId, PaymentMethod);

			_address = new AddressResource
			{
				region = "Saskatchewan",
				regionId = SaskRegionId,
				regionCode = "SK",
				countryId = "CA",
				street = new List<string> {"123 Fake Street"},
				telephone = "5555555555",
				postcode = "H0H0H0",
				city = "Regina",
				firstname = "Joe",
				lastname = "Blow",
				email = "joe@blow.com"
			};

			_shippingToSet = new CartSetShippingInformationResource(ShippingMethod, _address);
		}

		/// <summary>
		/// This test ensures the request does not error. If this test fails, either the test variables provided are invalid or the request errored 
		/// </summary>
		[TestMethod]
		public void CartController_AddItemToCart()
		{
			Assert.IsNotNull(_cartController.AddItemToCart(CartId, _itemToAdd));
		}

		/// <summary>
		/// This test ensures that an error occurs when null is attempted to be added to the cart
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CartController_AddItemToCart_NullItem()
		{
			Assert.IsNotNull(_cartController.AddItemToCart(CartId, null));
		}

		/// <summary>
		/// This test ensures that an error occurs when the item is not properly populated
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void CartController_AddItemToCart_InvalidItem()
		{
			_itemToAdd.cartItem.sku = null;
			Assert.IsNotNull(_cartController.AddItemToCart(CartId, _itemToAdd));
		}

		/// <summary>
		/// This test ensures the request does not error. If this test fails, either the test variables provided are invalid or the request errored 
		/// </summary>
		[TestMethod]
		public void CartController_AddPaymentMethod()
		{
			var cartModifiedId = _cartController.AddPaymentMethod(CartId, _methodToAdd);
			Assert.AreEqual(CartId, cartModifiedId);
		}

		/// <summary>
		/// This test ensures the request does not error. If this test fails, either the test variables provided are invalid or the request errored 
		/// </summary>
		[TestMethod]
		public void CartController_CreateCart()
		{
			Assert.IsNotNull(_cartController.CreateCart(CustomerId));
		}

		/// <summary>
		/// This test ensures the request does not error. If this test fails, either the test variables provided are invalid or the request errored 
		/// </summary>
		[TestMethod]
		public void CartController_SetShippingInformation()
		{
			Assert.IsNotNull(_cartController.SetShippingInformation(CartId, _shippingToSet));
		}

		/// <summary>
		/// This test ensures the request does not error. If this test fails, either the test variables provided are invalid or the request errored 
		/// </summary>
		[TestMethod]
		public void CartController_GetCart()
		{
			Assert.AreEqual(CartId, _cartController.GetCart(CartId).id);
		}

		/// <summary>
		/// This test ensures the request does not error. If this test fails, either the test variables provided are invalid or the request errored 
		/// </summary>
		[TestMethod]
		public void CartController_GetCartItems()
		{
			_cartController.AddItemToCart(CartId, _itemToAdd);
			Assert.IsTrue(_cartController.GetCartItems(CartId).Any());
		}

		/// <summary>
		/// This test ensures the request does not error. If this test fails, either the test variables provided are invalid or the request errored 
		/// </summary>
		[TestMethod]
		public void CartController_GetPaymentMethods()
		{
			Assert.IsNotNull(_cartController.GetPaymenMethods(CartId));
		}

		/// <summary>
		/// This test ensures the request does not error. If this test fails, either the test variables provided are invalid or the request errored 
		/// </summary>
		[TestMethod]
		public void CartController_GetShippingMethods()
		{
			Assert.IsNotNull(_cartController.GetShippingMethods(CartId));
		}

		/// <summary>
		/// This test ensures the request does not error. If this test fails, either the test variables provided are invalid or the request errored
		/// 
		/// Entire cart process must be completed before creating an order:
		///		Create Cart
		///		Add Item
		///		Set Shipping
		///		Add payment method
		///		Create Order
		/// </summary>
		[TestMethod]
		public void CartController_CreateOrder()
		{
			var cartIdForOrder = _cartController.CreateCart(CustomerId);

			//Adjust quote id since this will be a new cart
			_itemToAdd.cartItem.quote_id = cartIdForOrder.ToString();

			_cartController.AddItemToCart(cartIdForOrder, _itemToAdd);
			_cartController.SetShippingInformation(cartIdForOrder, _shippingToSet);

			//Adjust cart id since this will be a new cart
			_methodToAdd.cartId = cartIdForOrder.ToString();

			_cartController.AddPaymentMethod(cartIdForOrder, _methodToAdd);

			Assert.IsNotNull(_cartController.CreateOrder(cartIdForOrder, _methodToAdd));
		}
	}
}
