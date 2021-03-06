﻿using System;
using System.Collections.Generic;
using System.Linq;
using MagentoSync.Controllers.EndlessAisle.Interfaces;
using MagentoSync.Controllers.Magento.Interfaces;
using MagentoSync.Models.EndlessAisle.Catalog;
using MagentoSync.Models.EndlessAisle.Entities;
using MagentoSync.Models.EndlessAisle.Orders;
using MagentoSync.Models.Magento.Cart;
using MagentoSync.Models.Magento.Country;
using MagentoSync.Models.Magento.Customer;
using MagentoSync.Models.Magento.Products;
using MagentoSync.Utilities;
using AddressResource = MagentoSync.Models.Magento.Cart.AddressResource;

namespace MagentoSync.Mappers
{
	public class OrderMapper : BaseMapper
	{
		private readonly IOrdersController _eaOrderController;
		private readonly ICatalogsController _eaCatalogsController;
						 
		private readonly ICartController _magentoCartController;
		private readonly IProductController _magentoProductController;

		public const string CreatedDateProperty = "CreatedDateUtc";

		public OrderMapper(IOrdersController ordersController, ICatalogsController catalogsController, ICartController cartController, IProductController productController)
		{
			_eaOrderController = ordersController;
			_eaCatalogsController = catalogsController;

			_magentoCartController = cartController;
			_magentoProductController = productController;
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

			var createdOrders = _eaOrderController.GetOrders(new Filter(CreatedDateProperty, createdAfter, FilterCondition.GreaterThan));
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

		/// <summary>
		/// Adds the items from an EA order to a Cart in Magento.
		/// If an item on the order cannot be found in Magento an exception is thrown.
		/// </summary>
		/// <param name="orderId">Order to get items for</param>
		/// <param name="cartId">Cart to add items to</param>
		public void AddOrderItemsToCart(string orderId, int cartId)
		{
			Guid result;
			if (!Guid.TryParse(orderId, out result))
			{
				throw new ArgumentException("Argument must be a valid Guid.", nameof(orderId));
			}
			IEnumerable<OrderItemResource> orderItems = _eaOrderController.GetOrderItems(orderId);
			foreach (var orderItem in orderItems)
			{
				CatalogItemResource catalogItem = _eaCatalogsController.GetCatalogItem(orderItem.ProductId);
				ProductResource product = _magentoProductController.SearchForProducts(ConfigReader.MappingCode, catalogItem.Slug,
					ConfigReader.MagentoEqualsCondition).items.FirstOrDefault();

				if(product == null)
					throw new Exception(string.Format("Magento product could not be found for product {0} on order {1} with mapping code {2}", 
						orderItem.SKU, orderId, catalogItem.Slug));

				CartAddItemResource cartAddItem = new CartAddItemResource(cartId, product.sku, orderItem.Quantity);
				_magentoCartController.AddItemToCart(cartId, cartAddItem);
			}
		}

		/// <summary>
		/// Sets the shipping and billing information on a Magento cart.
		/// Shipping address used is based on the EA location unless the data needed is not available. In this case, data is pulled from the customer.
		/// Shipping method used will be the one set in App.config. If that shipping method cannot be used for the cart specified, an exception will occur.
		/// </summary>
		/// <param name="cartId">Identifier for cart to set information on.</param>
		/// <param name="magentoRegion">Region information for address</param>
		/// <param name="eaLocation">Location information for address</param>
		/// <param name="customer">Customer for address information</param>
		public void SetShippingAndBillingInformationForCart(int cartId, RegionResource magentoRegion, LocationResource eaLocation, CustomerResource customer)
		{
			AddressResource shippingAddress;
			if (magentoRegion != null)
				shippingAddress = new AddressResource(magentoRegion, eaLocation, customer);
			else
				shippingAddress = new AddressResource(customer);

			//Verfiy that shipping code matches App.config file
			var shippingInformation = new CartSetShippingInformationResource(ConfigReader.MagentoShippingCode, shippingAddress);
			_magentoCartController.SetShippingInformation(cartId, shippingInformation);

			if(_magentoCartController.GetShippingMethods(cartId).FirstOrDefault(x => x.method_code == ConfigReader.MagentoShippingCode) == null)
				throw new Exception(string.Format("Unable to add shipping information to cart {0}. Ensure that shipping code {1} is valid for this cart.", 
					cartId, ConfigReader.MagentoShippingCode));
		}

		/// <summary>
		/// Adds the payment method specified in App.config to the cart and creates an order from the cart.
		/// If the payment method is unable to be added to the cart then an exception will occur.
		/// </summary>
		/// <param name="cartId">Cart to create order from</param>
		/// <returns>Order ID created in Magento.</returns>
		public int CreateOrderForCart(int cartId)
		{
			var paymentMethod = new CartAddPaymentMethodResource(cartId, ConfigReader.MagentoPaymentMethod);
			if (_magentoCartController.GetPaymenMethods(cartId).Any(x => x.code == ConfigReader.MagentoPaymentMethod))
			{
				_magentoCartController.AddPaymentMethod(cartId, paymentMethod);
				return _magentoCartController.CreateOrder(cartId, paymentMethod);
			}
			else
			{
				throw new Exception(string.Format("Unable to create Order for cart {0}. " +
				                                  "\nNo payment method matching {1} found for cart. " +
				                                  "\nEnsure that Magento_PaymentMethod is valid in App.config", cartId, ConfigReader.MagentoPaymentMethod));
			}
		}
	}
}
