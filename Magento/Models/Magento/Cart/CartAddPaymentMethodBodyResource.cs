using System;
//// ReSharper disable InconsistentNaming
namespace MagentoSync.Models.Magento.Cart
{
	[Serializable]
	public class CartAddPaymentMethodBodyResource
	{
		public string method { get; set; }
	}
}