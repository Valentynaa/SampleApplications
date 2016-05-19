using System;
using System.Collections.Generic;

//// ReSharper disable InconsistentNaming
namespace MagentoConnect.Models.Magento.Cart
{
	[Serializable]
	public class BundleOptionResource
	{
		public int option_id { get; set; }// Bundle option id. ,
		public int option_qty { get; set; }// Bundle option quantity. ,
		public List<int> option_selections { get; set; }// Bundle option selection ids. ,
		public object extension_attributes = null;
	}
}