using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagentoSync.Models.EndlessAisle.Entities;
using MagentoSync.Models.Magento.Country;
using MagentoSync.Models.Magento.Customer;

// ReSharper disable InconsistentNaming
namespace MagentoSync.Models.Magento.Cart
{
	[Serializable]
	public class CartSetShippingInformationResource
	{
		public CartSetShippingInformationResource(string shippingCode, AddressResource shippingAddress, AddressResource billingAddress = null)
		{
			addressInformation = new CartSetShippingInformationBodyResource(shippingCode, shippingAddress, billingAddress);
		}

		public CartSetShippingInformationBodyResource addressInformation { get; set; }
	}
}
