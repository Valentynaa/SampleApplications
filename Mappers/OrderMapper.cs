using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagentoConnect.Controllers.EndlessAisle;
using MagentoConnect.Controllers.Magento;
using MagentoConnect.Models.EndlessAisle.Catalog;
using MagentoConnect.Models.EndlessAisle.Entities;
using MagentoConnect.Models.EndlessAisle.Order;
using MagentoConnect.Models.Magento.Cart;
using MagentoConnect.Models.Magento.Country;
using MagentoConnect.Models.Magento.Customer;
using MagentoConnect.Models.Magento.Products;
using MagentoConnect.Utilities;
using AddressResource = MagentoConnect.Models.Magento.Cart.AddressResource;

namespace MagentoConnect.Mappers
{
	public class OrderMapper : BaseMapper
	{
		private readonly OrderController _eaOrderController;
		private readonly CatalogsController _eaCatalogsController;

		private readonly CartController _magentoCartController;
		private readonly ProductController _magentoProductController;

		public OrderMapper(string magentoAuthToken, string eaAuthToken) : base(magentoAuthToken, eaAuthToken)
		{
			_eaOrderController = new OrderController(eaAuthToken);
			_eaCatalogsController = new CatalogsController(eaAuthToken);

			_magentoCartController = new CartController(magentoAuthToken);
			_magentoProductController = new ProductController(magentoAuthToken);
		}

		/// <summary>
		/// Returns a list of the EA orders that have been created since the specified time
		/// </summary>
		/// <param name="createdAfter">Get orders created after this time</param>
		/// <returns>List of the EA orders that match the time criteria</returns>
		public IEnumerable<OrderResource> GetEaOrdersCreatedAfter(DateTime createdAfter)
		{
			if (createdAfter > DateTime.UtcNow)
				throw new ArgumentOutOfRangeException(nameof(createdAfter));

			var createdOrders = _eaOrderController.GetOrders(new Filter("CreatedDateUtc", createdAfter, FilterCondition.GreaterThan));

			//Orders created after update
			return createdOrders;
		}

		/// <summary>
		/// Creates the cart for the customer specified in App.config
		/// </summary>
		/// <returns>Cart ID for created cart</returns>
		public int CreateCustomerCart()
		{
			return _magentoCartController.CreateCart(ConfigReader.MagentoCustomerId);
		}

		public void AddOrderItemsToCart(string orderId, int cartId)
		{
			IEnumerable<OrderItemResource> orderItems = _eaOrderController.GetOrderItems(orderId);
			foreach (var orderItem in orderItems)
			{
				CatalogItemResource catalogItem = _eaCatalogsController.GetCatalogItem(orderItem.ProductId);
				ProductResource product = _magentoProductController.SearchForProducts(ConfigReader.MappingCode, catalogItem.Slug,
					ConfigReader.MagentoEqualsCondition).items.FirstOrDefault();

				if(product == null)
					throw new Exception(string.Format("Magento product could not be found for product {0} on order {1} with mapping code {2}", orderItem.SKU, orderId, catalogItem.Slug));

				CartAddItemResource cartAddItem = new CartAddItemResource(cartId, product.sku, orderItem.Quantity);
				_magentoCartController.AddItemToCart(cartId, cartAddItem);
			}
		}

		/// <summary>
		/// Sets the shipping and billing information on a Magento cart.
		/// Shipping method used will be the one set in App.config. If that shipping method cannot be used for the cart specified, an exception will occur.
		/// </summary>
		/// <param name="cartId">Cart to set information on.</param>
		/// <param name="magentoRegion">Region for address information</param>
		/// <param name="eaLocation">Location for address information</param>
		/// <param name="customer">Customer for address information</param>
		public void SetShippingAndBillingInformationForCart(int cartId, RegionResource magentoRegion, LocationResource eaLocation, CustomerResource customer)
		{
			var shippingAddress = new AddressResource(magentoRegion, eaLocation, customer);

			//Verfiy that shipping code matches App.config file
			try
			{
				var shippingMethod =
					_magentoCartController.GetShippingMethods(cartId).First(x => x.method_code == ConfigReader.MagentoShippingCode);
				var shippingInformation = new CartSetShippingInformationResource(shippingMethod, shippingAddress);
				_magentoCartController.SetShippingInformation(cartId, shippingInformation);
			}
			catch (InvalidOperationException)
			{
				throw new Exception(string.Format("Unable to add shipping information to cart {0}. Ensure that shipping code {1} is valid for this cart.", cartId, ConfigReader.MagentoShippingCode));
			}
		}

		/// <summary>
		/// Adds the payment method specified in App.config to the cart and creates an order from the cart.
		/// If the payment method is unable to be added to the cart then an exception will occur.
		/// </summary>
		/// <param name="cartId">Cart to create order from</param>
		public void CreateOrderForCart(int cartId)
		{
			var paymentMethod = new CartAddPaymentMethodResource(cartId, ConfigReader.MagentoPaymentMethod);
			if (_magentoCartController.GetPaymenMethods(cartId).Any(x => x.code == ConfigReader.MagentoPaymentMethod))
			{
				_magentoCartController.AddPaymentMethod(cartId, paymentMethod);
				_magentoCartController.CreateOrder(cartId, paymentMethod);
			}
			else
			{
				throw new Exception(string.Format("Unable to create Order for cart {0}. No payment method mtching {1} found for cart.", cartId, ConfigReader.MagentoPaymentMethod));
			}
		}
	}
}
