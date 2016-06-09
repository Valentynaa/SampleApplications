using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable InconsistentNaming
namespace MagentoSync.Models.Magento.Cart
{
	[Serializable]
	public class CartShippingResponseResource
	{
		public IEnumerable<PaymentMethodResource> payment_methods { get; set; }
		public TotalsResource totals { get; set; }

		public object extension_attributes = null;
	}
}
