using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagentoConnect.Models.EndlessAisle.Order
{
	[Serializable]
	public class OrderItemSearchResultResource
	{
		public IEnumerable<OrderItemResource> Items { get; set; }
	}
}
