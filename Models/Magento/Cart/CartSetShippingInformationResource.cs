using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// ReSharper disable InconsistentNaming
namespace MagentoConnect.Models.Magento.Cart
{
	[Serializable]
	public class CartSetShippingInformationResource
	{
		public CartSetShippingInformationBodyResource addressInformation { get; set; }
	}
}
