using System;
using System.Collections.Generic;
using MagentoConnect.Models.Magento.CustomAttributes;

// ReSharper disable InconsistentNaming
namespace MagentoConnect.Models.Magento.Cart
{
	[Serializable]
	public class AddressResource
	{
		public string region { get; set; }// Region name,
		public int regionId { get; set; }// Region id,
		public string regionCode { get; set; }// Region code,
		public string countryId { get; set; }// Country id,
		public IEnumerable<string> street { get; set; }// Street,
		public string telephone { get; set; }// Telephone number,
		public string postcode { get; set; }// Postcode,
		public string city { get; set; }// City name,
		public string firstname { get; set; }// First name,
		public string lastname { get; set; }// Last name,
		public string email { get; set; }// Billing / shipping email,
	}
}