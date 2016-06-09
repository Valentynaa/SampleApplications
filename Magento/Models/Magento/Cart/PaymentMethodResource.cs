using System;
//// ReSharper disable InconsistentNaming
namespace MagentoSync.Models.Magento.Cart
{
	[Serializable]
	public class PaymentMethodResource
	{
		public string code { get; set; }
		public string title { get; set; }
	}
}
