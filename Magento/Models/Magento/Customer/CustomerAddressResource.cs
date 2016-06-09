using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagentoSync.Models.Magento.Products;

//// ReSharper disable InconsistentNaming
namespace MagentoSync.Models.Magento.Customer
{
	[Serializable]
	public class CustomerAddressResource
	{
		public int? id { get; set; }// ID ,
		public int? customer_id { get; set; }// Customer ID,
		public CustomerRegionResource region { get; set; }
		public int? region_id{ get; set; }// Region ID,
		public string country_id { get; set; }// Country code in ISO_3166-2 format ,
		public List<string> street{ get; set; }// Street ,
		public string company{ get; set; }// Company ,
		public string telephone{ get; set; }// Telephone number,
		public string fax{ get; set; }// Fax number,
		public string postcode{ get; set; }// Postcode ,
		public string city{ get; set; }// City name,
		public string firstname{ get; set; }// First name,
		public string lastname{ get; set; }// Last name,
		public string middlename{ get; set; }// Middle name,
		public string prefix{ get; set; }// Prefix ,
		public string suffix{ get; set; }// Suffix ,
		public string vat_id{ get; set; }// Vat id,
		public bool? default_shipping { get; set; }// If this address is default shipping address. ,
		public bool? default_billing{ get; set; }// If this address is default billing address,
		public object extension_attributes = null;
		public List<CustomAttributeRefResource> custom_attributes{ get; set; }// Custom attributes values.
	}
}
