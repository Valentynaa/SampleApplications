using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagentoConnect.Models.Magento.Cart;
using Newtonsoft.Json;
using RestSharp;

namespace MagentoConnect.Controllers.Magento
{
	public class CartController : BaseController
	{
		public static string MagentoAuthToken;

		public CartController(string magentoAuthToken)
		{
			MagentoAuthToken = magentoAuthToken;
		}

		/// <summary>
		/// Creates a cart / quote for the customer specified in Magento and returns that cart's ID
		/// </summary>
		/// <param name="customerId">Customer to create cart for</param>
		/// <returns>Cart ID for cart created</returns>
		public int CreateCart(int customerId)
		{
			var endpoint = UrlFormatter.MagentoCreateCartUrl(customerId);

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.POST);

			request.AddHeader("Authorization", string.Format("Bearer {0}", MagentoAuthToken));
			request.AddHeader("Content-Type", "application/json");

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode);

			return JsonConvert.DeserializeObject<int>(response.Content);
		}

		/// <summary>
		/// Adds an item to a cart in Magento and returns the CartItemResource created
		/// </summary>
		/// <param name="cartId">Cart to add item to</param>
		/// <param name="item">Item to add to cart</param>
		/// <returns>Cart Item that was added</returns>
		public CartItemResource AddItemToCart(int cartId, CartAddItemResource item)
		{
			var endpoint = UrlFormatter.MagentoAddItemToCartUrl(cartId);

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.POST);

			request.AddHeader("Authorization", string.Format("Bearer {0}", MagentoAuthToken));
			request.AddHeader("Content-Type", "application/json");

			request.AddJsonBody(item);

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode);

			return JsonConvert.DeserializeObject<CartItemResource>(response.Content);
		}

		/// <summary>
		/// Sets the shipping information for a cart in Magento
		/// </summary>
		/// <param name="cartId">Cart to set shipping information on</param>
		/// <param name="shippingInformation">Information to set</param>
		/// <returns>Response from setting information. Includes billing and shipping information</returns>
		public CartShippingResponseResource SetShippingInformation(int cartId, CartSetShippingInformationResource shippingInformation)
		{
			var endpoint = UrlFormatter.MagentoSetShippingInformationUrl(cartId);

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.POST);

			request.AddHeader("Authorization", string.Format("Bearer {0}", MagentoAuthToken));
			request.AddHeader("Content-Type", "application/json");

			request.AddJsonBody(shippingInformation);

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode);

			return JsonConvert.DeserializeObject<CartShippingResponseResource>(response.Content);
		}

		/// <summary>
		/// Gets the available shipping methods for a cart
		/// </summary>
		/// <param name="cartId">cart to get shipping methods for</param>
		/// <returns>List of shipping methods for cart</returns>
		public IEnumerable<ShippingMethodResource> GetShippingMethods(int cartId)
		{
			var endpoint = UrlFormatter.MagentoGetShippingMethodsForCartUrl(cartId);

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.GET);

			request.AddHeader("Authorization", string.Format("Bearer {0}", MagentoAuthToken));
			request.AddHeader("Content-Type", "application/json");

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode);

			return JsonConvert.DeserializeObject<IEnumerable<ShippingMethodResource>>(response.Content);
		}

		/// <summary>
		/// Gets the available payment methods for a cart
		/// </summary>
		/// <param name="cartId">cart to get payment methods for</param>
		/// <returns>List of payment methods for cart</returns>
		public IEnumerable<PaymentMethodResource> GetPaymenMethods(int cartId)
		{
			var endpoint = UrlFormatter.MagentoGetPaymentMethodsUrl(cartId);

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.GET);

			request.AddHeader("Authorization", string.Format("Bearer {0}", MagentoAuthToken));
			request.AddHeader("Content-Type", "application/json");

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode);

			return JsonConvert.DeserializeObject<IEnumerable<PaymentMethodResource>>(response.Content);
		}

		/// <summary>
		/// Adds a payment method to a cart.
		/// </summary>
		/// <param name="cartId">cart to add payment to</param>
		/// <param name="paymentMethod">Payment method to add</param>
		/// <returns>Cart ID of modified cart</returns>
		public int AddPaymentMethod(int cartId, CartAddPaymentMethodResource paymentMethod)
		{
			var endpoint = UrlFormatter.MagentoAddPaymentMethodUrl(cartId);

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.PUT);

			request.AddHeader("Authorization", string.Format("Bearer {0}", MagentoAuthToken));
			request.AddHeader("Content-Type", "application/json");

			request.AddJsonBody(paymentMethod);

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode);

			return JsonConvert.DeserializeObject<int>(response.Content);
		}

		/// <summary>
		/// Creates an Order in Magento from a cart with a specified payment method
		/// </summary>
		/// <param name="cartId">cart to add payment to</param>
		/// <param name="paymentMethod">Payment method to add</param>
		/// <returns>Order ID of order created</returns>
		public int CreateOrder(int cartId, CartAddPaymentMethodResource paymentMethod)
		{
			var endpoint = UrlFormatter.MagentoCreateAnOrderUrl(cartId);

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.PUT);

			request.AddHeader("Authorization", string.Format("Bearer {0}", MagentoAuthToken));
			request.AddHeader("Content-Type", "application/json");

			request.AddJsonBody(paymentMethod);

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode);

			return JsonConvert.DeserializeObject<int>(response.Content);
		}

		/// <summary>
		/// Gets the magento cart specified
		/// </summary>
		/// <param name="cartId">Cart to get</param>
		/// <returns>Magento Cart</returns>
		public CartResource GetCart(int cartId)
		{
			var endpoint = UrlFormatter.MagentoGetCartUrl(cartId);

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.GET);

			request.AddHeader("Authorization", string.Format("Bearer {0}", MagentoAuthToken));
			request.AddHeader("Content-Type", "application/json");

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode);

			return JsonConvert.DeserializeObject<CartResource>(response.Content);
		}

		/// <summary>
		/// Gets the items in a Magento cart
		/// </summary>
		/// <param name="cartId">Cart to get items from </param>
		/// <returns>Cart's items</returns>
		public IEnumerable<CartItemResource> GetCartItems(int cartId)
		{
			var endpoint = UrlFormatter.MagentoGetItemsInCartUrl(cartId);

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.GET);

			request.AddHeader("Authorization", string.Format("Bearer {0}", MagentoAuthToken));
			request.AddHeader("Content-Type", "application/json");

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode);

			return JsonConvert.DeserializeObject<IEnumerable<CartItemResource>>(response.Content);
		}
	}
}
