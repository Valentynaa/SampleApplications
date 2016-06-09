using System;
using System.Collections.Generic;

//// ReSharper disable InconsistentNaming
namespace MagentoSync.Models.Magento.Cart
{
	[Serializable]
	public class BundleOptionResource
	{
		/// <summary>
		/// Bundle option id.
		/// </summary>
		public int option_id { get; set; }

		/// <summary>
		/// Bundle option quantity. 
		/// </summary>
		public int option_qty { get; set; }

		/// <summary>
		/// Bundle option selection ids. ,
		/// </summary>
		public List<int> option_selections { get; set; }
		public object extension_attributes = null;
	}
}