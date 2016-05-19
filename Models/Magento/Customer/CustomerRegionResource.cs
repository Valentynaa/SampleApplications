using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//// ReSharper disable InconsistentNaming
namespace MagentoConnect.Models.Magento.Customer
{
	[Serializable]
	public class CustomerRegionResource
	{
		public string region_code { get; set; }// Region code,
		public string region { get; set; }// Region,
		public int region_id { get; set; }// Region id,
		public object extensionAttributes = null;
	}
}
