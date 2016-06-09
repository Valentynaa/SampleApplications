using System;
//// ReSharper disable InconsistentNaming
namespace MagentoSync.Models.Magento.Country
{
	[Serializable]
	public class RegionResource
	{
		public string id { get; set; }// Region id,
		public string code { get; set; }// Region code,
		public string name { get; set; }// Region name,
		public object extension_attributes = null;
	}
}