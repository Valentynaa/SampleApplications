using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagentoConnect.Models.EndlessAisle.Entities;
using MagentoConnect.Models.Magento.Country;
using MagentoConnect.Models.Magento.Customer;

// ReSharper disable InconsistentNaming
namespace MagentoConnect.Models.Magento.Cart
{
	[Serializable]
	public class CartSetShippingInformationResource
	{
		public CartSetShippingInformationResource(ShippingMethodResource shippingMethod, AddressResource shippingAddress, AddressResource billingAddress = null)
		{
			addressInformation = new CartSetShippingInformationBodyResource(shippingMethod, shippingAddress, billingAddress);
		}

		public CartSetShippingInformationBodyResource addressInformation { get; set; }
	}
}
