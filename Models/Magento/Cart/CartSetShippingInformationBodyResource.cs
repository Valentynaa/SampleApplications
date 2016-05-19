using System;
using System.Collections.Generic;
using MagentoConnect.Models.Magento.CustomAttributes;

// ReSharper disable InconsistentNaming
namespace MagentoConnect.Models.Magento.Cart
{
	[Serializable]
	public class CartSetShippingInformationBodyResource
	{
		public AddressResource shipping_address { get; set; }
		public AddressResource billing_address { get; set; }
		public string shipping_method_code { get; set; }// Shipping method code ,
		public string shipping_carrier_code { get; set; }// Carrier code,
		public IEnumerable<CustomAttributeResource> custom_attributes { get; set; }// Custom attributes values.
	}
}