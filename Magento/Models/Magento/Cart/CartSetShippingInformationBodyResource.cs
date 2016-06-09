using System;
using System.Collections.Generic;
using MagentoSync.Models.EndlessAisle.Entities;
using MagentoSync.Models.Magento.Country;
using MagentoSync.Models.Magento.CustomAttributes;
using MagentoSync.Models.Magento.Customer;

// ReSharper disable InconsistentNaming
namespace MagentoSync.Models.Magento.Cart
{
	[Serializable]
	public class CartSetShippingInformationBodyResource
	{
		public CartSetShippingInformationBodyResource(string shippingCode, AddressResource shippingAddress, AddressResource billingAddress = null)
		{
			this.shippingAddress = shippingAddress;
			this.billingAddress = billingAddress ?? shippingAddress;
			shippingCarrierCode = shippingCode;
			shippingMethodCode = shippingCode;
		}

		public AddressResource shippingAddress { get; set; }
		public AddressResource billingAddress { get; set; }
		public string shippingMethodCode { get; set; }// Shipping method code ,
		public string shippingCarrierCode { get; set; }// Carrier code,
	}
}