using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable InconsistentNaming
namespace MagentoSync.Models.Magento.Cart
{
	[Serializable]
	public class CartAddItemResource
	{
		public CartAddItemResource(int cartId, string sku, decimal qty)
		{
			cartItem = new CartAddItemBodyResource
			{
				quote_id = cartId.ToString(),
				sku = sku,
				qty = qty
			};
		}
		public CartAddItemBodyResource cartItem { get; set; }
	}
}
