using System;
// ReSharper disable InconsistentNaming
namespace MagentoConnect.Models.Magento.Cart
{
	[Serializable]
	public class TotalsSegmentResource
	{
		public string code { get; set; }// Code ,
		public string title { get; set; }// Total title,
		public decimal value { get; set; }// Total value,
		public string area { get; set; }// Display area code. ,
		public object extension_attributes = null;
	}
}