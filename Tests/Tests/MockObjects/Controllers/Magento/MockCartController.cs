using MagentoConnect.Controllers.Magento;
using MagentoConnect.Models.Magento.Cart;
using MagentoConnect.Models.Magento.Customer;
using System.Collections.Generic;

namespace Tests.MockObjects.Controllers.Magento
{
	public class MockCartController : BaseMockController, ICartController
	{
		/// <summary>
		/// Cart ID used for tests
		/// </summary>
		public static int CartId
		{
			get { return 3; }
		}

		/// <summary>
		/// Region ID used for cart tests
		/// </summary>
		public static int MappedRegionId
		{
			get { return 77; }
		}

		/// <summary>
		/// Region ID used for cart tests
		/// </summary>
		public static string ItemSku
		{
			get { return "Configurable Product"; }
		}

		/// <summary>
		/// Payment method used for cart tests
		/// </summary>
		public static string PaymentMethod
		{
			get { return "checkmo"; }
		}

		/// <summary>
		/// Shipping method used for cart tests
		/// </summary>
		public static string ShippingMethod
		{
			get { return "flatrate"; }
		}

		public int CreateCart(int customerId)
		{
			return 3;
		}

		public CartItemResource AddItemToCart(int cartId, CartAddItemResource item)
		{
			return new CartItemResource()
			{
				qty = item.cartItem.qty,
				sku = item.cartItem.sku,
				item_id = 5,
				price = new decimal(4.51),
				product_type = "simple",
				quote_id = item.cartItem.quote_id
			};
		}

		public CartShippingResponseResource SetShippingInformation(int cartId, CartSetShippingInformationResource shippingInformation)
		{
			return new CartShippingResponseResource()
			{
				payment_methods = new List<PaymentMethodResource>()
				{
					new PaymentMethodResource()
					{
						code = "checkmo",
						title = "Check / Money"
					},
					new PaymentMethodResource()
					{
						code = "free",
						title = "No Payment Information Required"
					}
				},
				totals = new TotalsResource()
				{
					grand_total = new decimal(184.91),
					base_grand_total = new decimal(184.91),
					subtotal = new decimal(184.91),
					base_subtotal = new decimal(184.91),
					subtotal_with_discount = new decimal(184.91),
					base_subtotal_with_discount = new decimal(184.91),
					subtotal_incl_tax = new decimal(184.91),
					base_currency_code = "USD",
					quote_currency_code = "USD",
					items_qty = 41,
					items = new List<TotalsItemResource>(),
					total_segments = new List<TotalsSegmentResource>()
				}
			};
		}

		public IEnumerable<ShippingMethodResource> GetShippingMethods(int cartId)
		{
			return new List<ShippingMethodResource>()
			{
				new ShippingMethodResource()
				{
					carrier_code = "flatrate",
					method_code = "flatrate",
					carrier_title = "Flat Rate",
					method_title = "Fixed",
					available = true,
					error_message = ""
				}
			};
		}

		public IEnumerable<PaymentMethodResource> GetPaymenMethods(int cartId)
		{
			return new List<PaymentMethodResource>()
			{
				new PaymentMethodResource()
				{
					code = "checkmo",
					title = "Check / Money"
				},
				new PaymentMethodResource()
				{
					code = "free",
					title = "No Payment Information Required"
				}
			};
		}

		public int AddPaymentMethod(int cartId, CartAddPaymentMethodResource paymentMethod)
		{
			return cartId;
		}

		public int CreateOrder(int cartId, CartAddPaymentMethodResource paymentMethod)
		{
			return cartId + 1;
		}

		public CartResource GetCart(int cartId)
		{
			return new CartResource()
			{
				id = cartId,
				created_at = "2016-05-17 22:10:51",
				updated_at = "2016-05-17 22:10:51",
				is_active = true,
				items_count = 1,
				items_qty = 41,
				customer = new CustomerResource()
				{
					created_at = "2016-05-17 22:10:51",
					updated_at = "2016-05-17 22:10:51",
					id = 1,
					group_id = 1,
					firstname = "Veronica",
					lastname = "Costello",
					addresses = new List<CustomerAddressResource>()
					{
						new CustomerAddressResource()
						{
							region = new CustomerRegionResource()
							{
								region_id = 33,
								region_code = "MI",
								region = "Michigan"
							},
							street = new List<string> { "123 Fake Street" },
							telephone = "5555555555",
							postcode = "H0H0H0",
							city = "Regina",
							firstname = "Joe",
							lastname = "Blow",
							default_billing = true,
							default_shipping = true
						}
					}
				},
				billing_address = new AddressResource
				{
					region = "Saskatchewan",
					regionId = 77,
					regionCode = "SK",
					countryId = "CA",
					street = new List<string> { "123 Fake Street" },
					telephone = "5555555555",
					postcode = "H0H0H0",
					city = "Regina",
					firstname = "Joe",
					lastname = "Blow",
					email = "joe@blow.com"
				},
				currency = new CurrencyResource()
				{
					global_currency_code = "USD",
					base_currency_code = "USD",
					quote_currency_code = "USD",
					store_currency_code = "USD"
				},
				customer_note_notify = true,
				customer_tax_class_id = 3,
				store_id = 1
			};
		}

		public IEnumerable<CartItemResource> GetCartItems(int cartId)
		{
			return new List<CartItemResource>()
			{
				new CartItemResource()
				{
					item_id = 5,
					sku = "Configurable Product",
					qty = 41,
					name = "Configurable Product",
					price = new decimal(4.51),
					quote_id = cartId.ToString(),
					product_type = "simple"
				}
			};
		}
	}
}
