using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagentoSync.Models.Magento.Cart
{
	[Serializable]
	public class PaymentMethodSearchResultResource
	{
		public IEnumerable<PaymentMethodResource> Items { get; set; }
	}
}
