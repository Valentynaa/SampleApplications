using System;
using System.Collections.Generic;

//// ReSharper disable InconsistentNaming
namespace MagentoSync.Models.Magento.Cart
{
	[Serializable]
	public class ProductOptionResource
	{
		public IEnumerable<CustomOptionResource> custom_options { get; set; }
		public IEnumerable<BundleOptionResource> bundle_options { get; set; }
		public DownloadableOptionResource downloadable_option { get; set; }
		public IEnumerable<ConfigurableItemOptionResource> configurable_item_options { get; set; }
	}
}