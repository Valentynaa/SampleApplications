using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagentoConnect.Models.Magento.Cart
{
	[Serializable]
	public class CartShippingMethodSearchResultResource
	{
		public IEnumerable<CartShippingMethodResource> Items { get; set; }
	}
}
