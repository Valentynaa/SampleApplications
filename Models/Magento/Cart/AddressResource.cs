using System;
using System.Collections.Generic;
using System.Linq;
using MagentoConnect.Models.EndlessAisle.Entities;
using MagentoConnect.Models.Magento.Country;
using MagentoConnect.Models.Magento.CustomAttributes;
using MagentoConnect.Models.Magento.Customer;

// ReSharper disable InconsistentNaming
namespace MagentoConnect.Models.Magento.Cart
{
	[Serializable]
	public class AddressResource
	{
		public AddressResource(RegionResource magentoRegion, LocationResource eaLocation, CustomerResource customer)
		{
			region = magentoRegion.name;
			regionId = int.Parse(magentoRegion.id);
			regionCode = magentoRegion.code;
			countryId = eaLocation.Address.CountryCode;
			street = customer.addresses.First().street;
			telephone = eaLocation.StorePhoneNumbers.First().Number;
			postcode = eaLocation.Address.Zip;
			city = eaLocation.Address.City;
			firstname = customer.firstname;
			lastname = customer.lastname;
			email = customer.email;
		}

		public AddressResource()
		{
		}

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