using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagentoConnect.Models.Magento.Products;

//// ReSharper disable InconsistentNaming
namespace MagentoConnect.Models.Magento.Customer
{
	[Serializable]
	public class CustomerResource
	{
		public int? id { get; set; }	// Customer id,
		public int? group_id { get; set; }	//Group id,
		public string default_billing { get; set; }//// Default billing address id,
		public string default_shipping { get; set; }// Default shipping address id,
		public string confirmation { get; set; }// Confirmation,
		public string created_at { get; set; }// Created at time,
		public string updated_at { get; set; }// Updated at time,
		public string created_in { get; set; }// Created in area,
		public string dob { get; set; }// Date of birth,
		public string email { get; set; }// Email address,
		public string firstname { get; set; }// First name,
		public string lastname { get; set; }// Last name,
		public string middlename { get; set; }// Middle name,
		public string prefix { get; set; }// Prefix,
		public string suffix { get; set; }// Suffix,
		public int? gender { get; set; }// Gender,
		public int? store_id { get; set; }// Store id,
		public string taxvat { get; set; }// Tax Vat,
		public int? website_id { get; set; }// Website id,
		public IEnumerable<CustomerAddressResource> addresses { get; set; }// Customer addresses. ,
		public int? disableAutoGroupChange { get; set; }// Disable auto group change flag. ,
		public object extension_attributes = null;
		public IEnumerable<CustomAttributeRefResource> custom_attributes { get; set; }// Custom attributes values.
	}
}
