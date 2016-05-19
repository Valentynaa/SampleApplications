using System;
//// ReSharper disable InconsistentNaming
namespace MagentoConnect.Models.Magento.Cart
{
	[Serializable]
	public class CartPaymentMethodResource
	{
		public string code { get; set; }
		public string title { get; set; }
	}
}
