using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagentoConnect;
using MagentoConnect.Controllers.Magento;
using MagentoConnect.Models.Magento.Cart;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Controllers.Magento
{
	[TestClass]
	public class CartControllerTests
	{
		//Private variables go here
		private CartController _cartController;
		private const int CartId = 3;
		private const string ItemSku = "Configurable Product";
		private const string PaymentMethod = "checkmo";
		private const string ShippingMethod = "flatrate";
		private const int CustomerId = 2;
		private CartAddItemResource _itemToAdd;
		private CartAddPaymentMethodResource _methodToAdd;
		private CartSetShippingInformationResource _shippingToSet;
		private AddressResource _address;

		[TestInitialize]
		public void SetUp()
		{
			string magentoAuthToken = App.GetMagentoAuthToken();
			_cartController = new CartController(magentoAuthToken);

			_itemToAdd = new CartAddItemResource(CartId, ItemSku, 1);

			_methodToAdd = new CartAddPaymentMethodResource(CartId, PaymentMethod);

			_address = new AddressResource
			{
				region = "Saskatchewan",
				regionId = 77,
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

		//Tests go here

		/// <summary>
		/// This test ensures the request does not error. If this test fails, either the test variables provided are invalid or the request errored 
		/// </summary>
		[TestMethod]
		public void CartController_AddItemToCart()
		{
			CartItemResource item = _cartController.AddItemToCart(CartId, _itemToAdd);
		}

		/// <summary>
		/// This test ensures the request does not error. If this test fails, either the test variables provided are invalid or the request errored 
		/// </summary>
		[TestMethod]
		public void CartController_AddPaymentMethod()
		{
			int cartModifiedId = _cartController.AddPaymentMethod(CartId, _methodToAdd);
			Assert.AreEqual(CartId, cartModifiedId);
		}

		/// <summary>
		/// This test ensures the request does not error. If this test fails, either the test variables provided are invalid or the request errored 
		/// </summary>
		[TestMethod]
		public void CartController_CreateCart()
		{
			int cartCreatedId = _cartController.CreateCart(CustomerId);
		}

		/// <summary>
		/// This test ensures the request does not error. If this test fails, either the test variables provided are invalid or the request errored 
		/// </summary>
		[TestMethod]
		public void CartController_SetShippingInformation()
		{
			CartShippingResponseResource shippingResponse = _cartController.SetShippingInformation(CartId, _shippingToSet);
		}

		/// <summary>
		/// This test ensures the request does not error. If this test fails, either the test variables provided are invalid or the request errored 
		/// </summary>
		[TestMethod]
		public void CartController_GetCart()
		{
			CartResource cart = _cartController.GetCart(CartId);
		}

		/// <summary>
		/// This test ensures the request does not error. If this test fails, either the test variables provided are invalid or the request errored 
		/// </summary>
		[TestMethod]
		public void CartController_GetCartItems()
		{
			IEnumerable<CartItemResource> cartItems = _cartController.GetCartItems(CartId);
		}

		/// <summary>
		/// This test ensures the request does not error. If this test fails, either the test variables provided are invalid or the request errored 
		/// </summary>
		[TestMethod]
		public void CartController_GetPaymentMethods()
		{
			IEnumerable<PaymentMethodResource> paymentMethods = _cartController.GetPaymenMethods(CartId);
		}

		/// <summary>
		/// This test ensures the request does not error. If this test fails, either the test variables provided are invalid or the request errored 
		/// </summary>
		[TestMethod]
		public void CartController_GetShippingMethods()
		{
			IEnumerable<ShippingMethodResource> shippingMethods = _cartController.GetShippingMethods(CartId);
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
			int cartIdForOrder = _cartController.CreateCart(CustomerId);

			//Adjust quote id since this will be a new cart
			_itemToAdd.cartItem.quote_id = cartIdForOrder.ToString();

			_cartController.AddItemToCart(cartIdForOrder, _itemToAdd);
			_cartController.SetShippingInformation(cartIdForOrder, _shippingToSet);

			//Adjust cart id since this will be a new cart
			_methodToAdd.cartId = cartIdForOrder.ToString();

			_cartController.AddPaymentMethod(cartIdForOrder, _methodToAdd);

			int orderCreatedId = _cartController.CreateOrder(cartIdForOrder, _methodToAdd);
		}
	}
}
