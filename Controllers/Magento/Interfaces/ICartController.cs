using System.Collections.Generic;
using MagentoConnect.Models.Magento.Cart;

namespace MagentoConnect.Controllers.Magento
{
	public interface ICartController : IController
	{
		/// <summary>
		/// Creates a cart / quote for the customer specified in Magento and returns that cart's ID
		/// </summary>
		/// <param name="customerId">Identifier of customer to create cart for</param>
		/// <returns>Cart ID for cart created</returns>
		int CreateCart(int customerId);

		/// <summary>
		/// Adds an item to a cart in Magento and returns the CartItemResource created
		/// 
		/// NOTE:
		///		Throws exception if invalid CartAddItemResource is passed in. CartAddItemResource is invalid 
		///		if the qoute_id or cart item sku are not set.
		/// </summary>
		/// <param name="cartId">Cart to add item to</param>
		/// <param name="item">Item to add to cart</param>
		/// <returns>Cart Item that was added</returns>
		CartItemResource AddItemToCart(int cartId, CartAddItemResource item);

		/// <summary>
		/// Sets the shipping information for a cart in Magento
		/// </summary>
		/// <param name="cartId">Cart to set shipping information on</param>
		/// <param name="shippingInformation">Information to set</param>
		/// <returns>Response from setting information. Includes billing and shipping information</returns>
		CartShippingResponseResource SetShippingInformation(int cartId, CartSetShippingInformationResource shippingInformation);

		/// <summary>
		/// Gets the available shipping methods for a cart
		/// </summary>
		/// <param name="cartId">cart to get shipping methods for</param>
		/// <returns>List of shipping methods for cart</returns>
		IEnumerable<ShippingMethodResource> GetShippingMethods(int cartId);

		/// <summary>
		/// Gets the available payment methods for a cart
		/// </summary>
		/// <param name="cartId">cart to get payment methods for</param>
		/// <returns>List of payment methods for cart</returns>
		IEnumerable<PaymentMethodResource> GetPaymenMethods(int cartId);

		/// <summary>
		/// Adds a payment method to a cart.
		/// </summary>
		/// <param name="cartId">cart to add payment to</param>
		/// <param name="paymentMethod">Payment method to add</param>
		/// <returns>Cart ID of modified cart</returns>
		int AddPaymentMethod(int cartId, CartAddPaymentMethodResource paymentMethod);

		/// <summary>
		/// Creates an Order in Magento from a cart with a specified payment method
		/// </summary>
		/// <param name="cartId">cart to add payment to</param>
		/// <param name="paymentMethod">Payment method to add</param>
		/// <returns>Order ID of order created</returns>
		int CreateOrder(int cartId, CartAddPaymentMethodResource paymentMethod);

		/// <summary>
		/// Gets the magento cart specified
		/// </summary>
		/// <param name="cartId">Cart to get</param>
		/// <returns>Magento Cart</returns>
		CartResource GetCart(int cartId);

		/// <summary>
		/// Gets the items in a Magento cart
		/// </summary>
		/// <param name="cartId">Cart to get items from </param>
		/// <returns>Cart's items</returns>
		IEnumerable<CartItemResource> GetCartItems(int cartId);
	}
}