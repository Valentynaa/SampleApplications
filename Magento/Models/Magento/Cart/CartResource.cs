using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagentoSync.Models.Magento.Customer;

//// ReSharper disable InconsistentNaming
namespace MagentoSync.Models.Magento.Cart
{
	[Serializable]
	public class CartResource
	{
		public int id { get; set; }// Cart/quote ID. ,
		public string created_at { get; set; }// Cart creation date and time.Otherwise, null. ,
		public string updated_at { get; set; }// Cart last update date and time.Otherwise, null. ,
		public string converted_at { get; set; }// Cart conversion date and time.Otherwise, null. ,
		public bool? is_active { get; set; }// Active status flag value.Otherwise, null. ,
		public bool? is_virtual { get; set; }// Virtual flag value.Otherwise, null. ,
		public IEnumerable<CartItemResource> items { get; set; }// Array of items.Otherwise, null. ,
		public int? items_count { get; set; }// Number of different items or products in the cart.Otherwise, null. ,
		public decimal? items_qty { get; set; }// Total quantity of all cart items.Otherwise, null. ,
		public CustomerResource customer { get; set; }
		public AddressResource billing_address { get; set; }
		public int? reserved_order_id { get; set; }// Reserved order ID.Otherwise, null. ,
		public int? orig_order_id { get; set; }// Original order ID.Otherwise, null. ,
		public CurrencyResource currency { get; set; }
		public bool? customer_is_guest { get; set; }// For guest customers, false for logged in customers ,
		public string customer_note { get; set; }// Notice text,
		public bool? customer_note_notify { get; set; }// Customer notification flag,
		public int? customer_tax_class_id { get; set; }// Customer tax class ID. ,
		public int store_id { get; set; }// Store identifier,
		public string extension_attributes = null;
	}
}
