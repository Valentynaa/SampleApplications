using System;
//// ReSharper disable InconsistentNaming
namespace MagentoSync.Models.Magento.Cart
{
	[Serializable]
	public class ConfigurableItemOptionResource
	{
		public string option_id { get; set; }// Option SKU,
		public int? option_value { get; set; }// Item id,
		public object extension_attributes = null;
	}
}