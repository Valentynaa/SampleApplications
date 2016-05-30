using System;
//// ReSharper disable InconsistentNaming
namespace MagentoConnect.Models.Magento.Cart
{
	[Serializable]
	public class CartAddPaymentMethodBodyResource
	{
		public string method { get; set; }
	}
}