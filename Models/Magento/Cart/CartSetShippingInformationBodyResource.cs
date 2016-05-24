using System;
using System.Collections.Generic;
using MagentoConnect.Models.EndlessAisle.Entities;
using MagentoConnect.Models.Magento.Country;
using MagentoConnect.Models.Magento.CustomAttributes;
using MagentoConnect.Models.Magento.Customer;

// ReSharper disable InconsistentNaming
namespace MagentoConnect.Models.Magento.Cart
{
	[Serializable]
	public class CartSetShippingInformationBodyResource
	{
		public CartSetShippingInformationBodyResource(ShippingMethodResource shippingMethod, AddressResource shippingAddress, AddressResource billingAddress = null)
		{
			this.shippingAddress = shippingAddress;
			this.billingAddress = billingAddress ?? shippingAddress;
			shippingCarrierCode = shippingMethod.carrier_code;
			shippingMethodCode = shippingMethod.method_code;
		}

		public AddressResource shippingAddress { get; set; }
		public AddressResource billingAddress { get; set; }
		public string shippingMethodCode { get; set; }// Shipping method code ,
		public string shippingCarrierCode { get; set; }// Carrier code,
	}
}