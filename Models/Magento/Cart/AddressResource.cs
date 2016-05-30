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
		/// <summary>
		/// Creates Address based on EA location. If the data is not there, default to customer data.
		/// </summary>
		/// <param name="magentoRegion"></param>
		/// <param name="eaLocation"></param>
		/// <param name="customer"></param>
		public AddressResource(RegionResource magentoRegion, LocationResource eaLocation, CustomerResource customer)
		{
			var customerAddress = customer.addresses.First();
			region = magentoRegion.name;
			regionId = int.Parse(magentoRegion.id);
			regionCode = magentoRegion.code;
			countryId = eaLocation.Address.CountryCode;

			street = new List<string>();
			if (eaLocation.Address.AddressLine1 != null)
				street.Add(eaLocation.Address.AddressLine1);
			if(eaLocation.Address.AddressLine2 != null)
				street.Add(eaLocation.Address.AddressLine2);

			if (!street.Any())
			{
				street = customerAddress.street;
			}

			telephone = eaLocation.StorePhoneNumbers.FirstOrDefault()?.Number ?? customerAddress.telephone;
			postcode = eaLocation.Address.Zip ?? customerAddress.postcode;
			city = eaLocation.Address.City;
			firstname = customer.firstname;
			lastname = customer.lastname;
			email = customer.email;
		}

		public AddressResource(CustomerResource customer)
		{
			var address = customer.addresses.First();
			region = address.region.region;
			regionId = address.region.region_id;
			regionCode = address.region.region_code;
			countryId = address.country_id;
			street = address.street;
			telephone = address.telephone;
			postcode = address.postcode;
			city = address.city;
			firstname = address.firstname;
			lastname = address.lastname;
			email = customer.email;
		}

		public AddressResource()
		{
		}

		/// <summary>
		/// Region name,
		/// </summary>
		public string region { get; set; }
		public int regionId { get; set; }// Region id,
		public string regionCode { get; set; }// Region code,
		public string countryId { get; set; }// Country id,
		public List<string> street { get; set; }// Street,
		public string telephone { get; set; }// Telephone number,
		public string postcode { get; set; }// Postcode,
		public string city { get; set; }// City name,
		public string firstname { get; set; }// First name,
		public string lastname { get; set; }// Last name,
		public string email { get; set; }// Billing / shipping email,
	}
}