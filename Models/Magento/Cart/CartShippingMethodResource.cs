using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//// ReSharper disable InconsistentNaming
namespace MagentoConnect.Models.Magento.Cart
{
	[Serializable]
	public class CartShippingMethodResource
	{
		public string carrier_code { get; set; }// Shipping carrier code. ,
		public string method_code { get; set; }// Shipping method code. ,
		public string carrier_title { get; set; }// Shipping carrier title.Otherwise, null. ,
		public string method_title { get; set; }// Shipping method title.Otherwise, null. ,
		public decimal amount { get; set; }// Shipping amount in store currency. ,
		public decimal base_amount { get; set; }// Shipping amount in base currency. ,
		public bool available { get; set; }// The value of the availability flag for the current shipping method. ,
		public object extension_attributes = null;
		public string error_message { get; set; }// Shipping Error message. ,
		public decimal price_excl_tax { get; set; }// Shipping price excl tax. ,
		public decimal price_incl_tax { get; set; }// Shipping price incl tax.
	}
}
