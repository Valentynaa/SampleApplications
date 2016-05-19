using System;
//// ReSharper disable InconsistentNaming
namespace MagentoConnect.Models.Magento.Cart
{
	[Serializable]
	public class CustomOptionResource
	{
		public string option_id { get; set; }
		public string option_value { get; set; }
		public object extension_attributes = null;
	}
}