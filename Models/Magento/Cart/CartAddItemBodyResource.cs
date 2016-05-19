using System;
// ReSharper disable InconsistentNaming
namespace MagentoConnect.Models.Magento.Cart
{
	[Serializable]
	public class CartAddItemBodyResource
	{
		public string sku { get; set; }// Product SKU.Otherwise, null. ,
		public decimal qty { get; set; }// Product quantity. ,
		public string quote_id { get; set; }// Quote id. ,
	}
}