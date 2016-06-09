using System;
//// ReSharper disable InconsistentNaming
namespace MagentoSync.Models.Magento.Cart
{
	[Serializable]
	public class CartItemResource
	{
		public int? item_id { get; set; }// Item ID.Otherwise, null. ,
		public string sku { get; set; }// Product SKU.Otherwise, null. ,
		public decimal qty  { get; set; }// Product quantity. ,
		public string name { get; set; }// Product name.Otherwise, null. ,
		public decimal? price { get; set; }// Product price.Otherwise, null. ,
		public string product_type { get; set; }// Product type.Otherwise, null. ,
		public string quote_id { get; set; }// Quote id. ,
		public ProductOptionResource product_option { get; set; }
		public object extension_attributes = null;
	}
}