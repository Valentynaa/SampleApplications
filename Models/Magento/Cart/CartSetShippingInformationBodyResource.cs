using System;
using System.Collections.Generic;
using MagentoConnect.Models.Magento.CustomAttributes;

// ReSharper disable InconsistentNaming
namespace MagentoConnect.Models.Magento.Cart
{
	[Serializable]
	public class CartSetShippingInformationBodyResource
	{
		public AddressResource shippingAddress { get; set; }
		public AddressResource billingAddress { get; set; }
		public string shippingMethodCode { get; set; }// Shipping method code ,
		public string shippingCarrierCode { get; set; }// Carrier code,
	}
}